using System.Data;
using AggieEnterpriseApi;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using AD419.Jobs.Configuration;
using Serilog;
using AD419.Jobs.Core.Utilities;
using CsvHelper;
using System.Globalization;
using CsvHelper.Configuration;
using System.Text.RegularExpressions;
using AD419.Jobs.Models;
using AD419.Jobs.Core.Extensions;
using AD419.Jobs.PullNifaData.Attributes;
using AD419.Jobs.PullNifaData.Extensions;
using FastMember;

namespace AD419.Jobs.PullNifaData.Services;

public class SyncService
{
    private readonly ConnectionStrings _connectionStrings;
    private readonly SyncOptions _syncOptions;
    private readonly ISshService _sshService;

    public SyncService(IOptions<ConnectionStrings> connectionStrings, IOptions<SyncOptions> syncOptions, ISshService sshService)
    {
        _connectionStrings = connectionStrings.Value;
        _syncOptions = syncOptions.Value;
        _sshService = sshService;
    }

    public async Task Run()
    {
        Log.Information("Starting sync");
        using var connection = new SqlConnection(_connectionStrings.DefaultConnection);
        await connection.OpenAsync();

        var filePaths = _sshService.ListFiles("/")
            .Where(f => Regex.IsMatch(Path.GetFileName(f), $@"NIFA_(GL|PGM_(AWARD|EMPLOYEE|EXPENDITURE|PROJECT))_Incremental_[0-9]{{8}}_[0-9]{{6}}\.csv"))
            .OrderBy(x => x);

        foreach (var filePath in filePaths)
        {
            Log.Information("Processing file {FileName}", filePath);
            using var transaction = (SqlTransaction)await connection.BeginTransactionAsync();
            try
            {
                var stream = _sshService.DownloadFile(filePath);
                stream.Position = 0;
                using var reader = new StreamReader(stream);
                using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    HasHeaderRecord = true,
                    Delimiter = "|",
                });

                switch (Path.GetFileName(filePath))
                {
                    case string s when Regex.IsMatch(s, @"NIFA_GL_.*?\.csv"):
                        await SyncData<NifaGlModel>(connection, transaction, csv, "UCD_NIFA_GL");
                        break;
                    case string s when Regex.IsMatch(s, @"NIFA_PGM_AWARD_.*?\.csv"):
                        await SyncData<NifaPgmAwardModel>(connection, transaction, csv, "UCD_NIFA_PGM_AWARD");
                        break;
                    case string s when Regex.IsMatch(s, @"NIFA_PGM_EMPLOYEE_.*?\.csv"):
                        await SyncData<NifaPgmEmployeeModel>(connection, transaction, csv, "UCD_NIFA_PGM_EMPLOYEE");
                        break;
                    case string s when Regex.IsMatch(s, @"NIFA_PGM_EXPENDITURE_.*?\.csv"):
                        await SyncData<NifaPgmExpenditureModel>(connection, transaction, csv, "UCD_NIFA_PGM_EXPENDITURE");
                        break;
                    case string s when Regex.IsMatch(s, @"NIFA_PGM_PROJECT_.*?\.csv"):
                        await SyncData<NifaPgmProjectModel>(connection, transaction, csv, "UCD_NIFA_PGM_PROJECT");
                        break;
                    default:
                        throw new Exception($"Unknown file type {Path.GetFileName(filePath)}");
                }

                _sshService.MoveFile(filePath, $"{_syncOptions.ProcessedFileLocation}/{Path.GetFileName(filePath)}");

                await transaction.CommitAsync();
            }
            catch (Exception e)
            {
                Log.Error(e, "Error syncing");
                try
                {
                    await transaction.RollbackAsync();
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "Error rolling back transaction");
                }
                throw;
            }
        }
    }

    private async Task SyncData<T>(SqlConnection connection, SqlTransaction transaction, CsvReader csv, string tableName)
        where T : class
    {
        var dataTable = CreateDataTable<T>();
        Log.Information("Reading {TableName} data", tableName);
        var records = csv.GetRecords<T>();
        foreach (var record in records)
        {
            dataTable.AddModelData(record);
        }
        Log.Information("Creating temp table {TableName}", tableName);
        await SqlHelper.ExecuteScript($"Scripts/{tableName}_init.sql", connection);
        Log.Information("Writing {TableName} data to temp table", tableName);
        var bulkCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.Default, transaction)
        {
            DestinationTableName = tableName,
            BatchSize = _syncOptions.BulkCopyBatchSize
        };
        await bulkCopy.WriteToServerAsync(dataTable);
        Log.Information("Merging {TableName} data", tableName);
        await SqlHelper.ExecuteScript($"Scripts/{tableName}_merge.sql", connection, transaction);
    }

    private static DataTable CreateDataTable<T>()
    {
        var dataTable = new DataTable();

        // Column order is important, since SqlBulkCopy works by ordinal position, not column name
        foreach (var property in typeof(T).GetOrderedProperties())
        {
            // SqlBulkCopy's type conversion doesn't understand Nullable<> types, so unwrap them
            var propertyType = property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>)
                ? Nullable.GetUnderlyingType(property.PropertyType)
                : property.PropertyType;

            dataTable.Columns.Add(property.Name, propertyType!);
        }

        return dataTable;
    }

}

public static class DataTableExtensions
{
    public static void AddModelData<T>(this DataTable dataTable, T model)
        where T : class
    {
        // TypeAccessors are cached, so it's okay to call this in tight loops
        var accessor = TypeAccessor.Create(typeof(T));
        var row = dataTable.NewRow();
        var properties = typeof(T).GetProperties();
        foreach (var property in properties)
        {
            // SqlBulkCopy needs nulls to be represented by DBNull.Value
            var value = accessor[model, property.Name] ?? DBNull.Value;
            row[property.Name] = value;
        }
        dataTable.Rows.Add(row);
    }
}