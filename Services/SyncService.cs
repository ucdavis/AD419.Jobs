using System.Collections.Concurrent;
using System.Data;
using AggieEnterpriseApi;
using AD419Functions.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using AD419Functions.Configuration;
using Serilog;
using System.Net;
using AD419Functions.Utilities;
using System.Text;

namespace AD419Functions.Services;

public class SyncService
{
    private readonly AggieEnterpriseService _aggieEnterpriseService;
    private readonly ConcurrentDictionary<Type, DataTable> _dataTables = new();
    private readonly ConnectionStrings _connectionStrings;
    private readonly SyncOptions _syncOptions;

    public SyncService(AggieEnterpriseService aggieEnterpriseService, IOptions<ConnectionStrings> connectionStrings, IOptions<SyncOptions> syncOptions)
    {
        _aggieEnterpriseService = aggieEnterpriseService;
        _connectionStrings = connectionStrings.Value;
        _syncOptions = syncOptions.Value;
    }

    public async Task Run()
    {
        Log.Information("Starting sync");
        using var connection = new SqlConnection(_connectionStrings.DefaultConnection);
        await connection.OpenAsync();
        await SyncFinancialDepartmentValues(connection);
    }

    public async Task SyncFinancialDepartmentValues(SqlConnection connection)
    {
        var dataTable = GetDataTable<IErpDepartmentAllPaged_ErpFinancialDepartmentSearch_Data>("#ErpFinancialDepartmentValues");
        await ExecuteScript("Scripts/ErpFinancialDepartmentValues_Create.sql", connection);

        var i = 0;
        await foreach (var item in _aggieEnterpriseService.GetFinancialDepartmentValues())
        {
            dataTable.Add(item);
            if (++i % _syncOptions.BulkCopyBatchSize == 0)
            {
                Log.Information("Syncing batch of {BatchSize} financial department values", _syncOptions.BulkCopyBatchSize);
                await SyncDataTable(connection, dataTable);
                dataTable.Rows.Clear();
            }
        }

        if (dataTable.Rows.Count > 0)
        {
            // one last batch
            Log.Information("Syncing batch of {BatchSize} financial department values", dataTable.Rows.Count);
            await SyncDataTable(connection, dataTable);
            dataTable.Rows.Clear();
        }

        await ExecuteScript("Scripts/ErpFinancialDepartmentValues_CopyToFinancialDepartmentValues.sql", connection);
    }

    private static async Task ExecuteScript(string fileName, SqlConnection connection)
    {
        Log.Information("Executing script {FileName}", fileName);
        // TODO: use connection.CreateBatch() once it is implemented for SqlConnection
        foreach (var script in SqlHelper.GetBatchesFromFile(fileName))
        {
            using var command = connection.CreateCommand();
            command.CommandText = script;
            await command.ExecuteNonQueryAsync();
        }
    }

    private static async Task SyncDataTable(SqlConnection connection, DataTable dataTable)
    {
        using var bulkCopy = new SqlBulkCopy(connection);
        bulkCopy.DestinationTableName = dataTable.TableName;
        bulkCopy.BatchSize = dataTable.Rows.Count;
        await bulkCopy.WriteToServerAsync(dataTable);
    }

    private DataTable<T> GetDataTable<T>(string destinationTableName)
    {
        var dataTable = (DataTable<T>)_dataTables.GetOrAdd(typeof(T), _ => new DataTable<T>(destinationTableName));
        dataTable.Rows.Clear();
        return dataTable;
    }
}