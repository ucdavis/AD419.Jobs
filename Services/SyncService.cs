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
        using var transaction = (SqlTransaction)await connection.BeginTransactionAsync();
        try
        {
            await SyncFinancialDepartmentValues(connection, transaction);
            await SyncFundValues(connection, transaction);
            await SyncAccountValues(connection, transaction);
            await SyncProjectValues(connection, transaction);
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

    private async Task SyncFinancialDepartmentValues(SqlConnection connection, SqlTransaction transaction)
    {
        var dataTable = GetDataTable<IErpDepartmentAllPaged_ErpFinancialDepartmentSearch_Data>("#ErpFinancialDepartmentValues");
        await ExecuteScript("Scripts/ErpFinancialDepartmentValues_Start.sql", connection, transaction);

        var i = 0;
        await foreach (var item in _aggieEnterpriseService.GetFinancialDepartmentValues())
        {
            dataTable.Add(item);
            if (++i % _syncOptions.BulkCopyBatchSize == 0)
            {
                Log.Information("Syncing batch of {BatchSize} financial department values", _syncOptions.BulkCopyBatchSize);
                await SyncDataTable(connection, dataTable, transaction);
                dataTable.Rows.Clear();
            }
        }

        if (dataTable.Rows.Count > 0)
        {
            // one last batch
            Log.Information("Syncing batch of {BatchSize} financial department values", dataTable.Rows.Count);
            await SyncDataTable(connection, dataTable, transaction);
            dataTable.Rows.Clear();
        }

        await ExecuteScript("Scripts/ErpFinancialDepartmentValues_Finish.sql", connection, transaction);
    }

    private async Task SyncFundValues(SqlConnection connection, SqlTransaction transaction)
    {
        var dataTable = GetDataTable<IErpFundAllPaged_ErpFundSearch_Data>("#ErpFundValues");
        await ExecuteScript("Scripts/ErpFundValues_Start.sql", connection, transaction);

        var i = 0;
        await foreach (var item in _aggieEnterpriseService.GetFundValues())
        {
            dataTable.Add(item);
            if (++i % _syncOptions.BulkCopyBatchSize == 0)
            {
                Log.Information("Syncing batch of {BatchSize} fund values", _syncOptions.BulkCopyBatchSize);
                await SyncDataTable(connection, dataTable, transaction);
                dataTable.Rows.Clear();
            }
        }

        if (dataTable.Rows.Count > 0)
        {
            // one last batch
            Log.Information("Syncing batch of {BatchSize} fund values", dataTable.Rows.Count);
            await SyncDataTable(connection, dataTable, transaction);
            dataTable.Rows.Clear();
        }

        await ExecuteScript("Scripts/ErpFundValues_Finish.sql", connection, transaction);
    }

    private async Task SyncAccountValues(SqlConnection connection, SqlTransaction transaction)
    {
        var dataTable = GetDataTable<IErpAccountAllPaged_ErpAccountSearch_Data>("#ErpAccountValues");
        await ExecuteScript("Scripts/ErpAccountValues_Start.sql", connection, transaction);

        var i = 0;
        await foreach (var item in _aggieEnterpriseService.GetAccountValues())
        {
            dataTable.Add(item);
            if (++i % _syncOptions.BulkCopyBatchSize == 0)
            {
                Log.Information("Syncing batch of {BatchSize} account values", _syncOptions.BulkCopyBatchSize);
                await SyncDataTable(connection, dataTable, transaction);
                dataTable.Rows.Clear();
            }
        }

        if (dataTable.Rows.Count > 0)
        {
            // one last batch
            Log.Information("Syncing batch of {BatchSize} account values", dataTable.Rows.Count);
            await SyncDataTable(connection, dataTable, transaction);
            dataTable.Rows.Clear();
        }

        await ExecuteScript("Scripts/ErpAccountValues_Finish.sql", connection, transaction);
    }

    private async Task SyncProjectValues(SqlConnection connection, SqlTransaction transaction)
    {
        var dataTable = GetDataTable<IErpProjectAllPaged_ErpProjectSearch_Data>("#ErpProjectValues");
        await ExecuteScript("Scripts/ErpProjectValues_Start.sql", connection, transaction);

        var i = 0;
        await foreach (var item in _aggieEnterpriseService.GetProjectValues())
        {
            dataTable.Add(item);
            if (++i % _syncOptions.BulkCopyBatchSize == 0)
            {
                Log.Information("Syncing batch of {BatchSize} Project values", _syncOptions.BulkCopyBatchSize);
                await SyncDataTable(connection, dataTable, transaction);
                dataTable.Rows.Clear();
            }
        }

        if (dataTable.Rows.Count > 0)
        {
            // one last batch
            Log.Information("Syncing batch of {BatchSize} Project values", dataTable.Rows.Count);
            await SyncDataTable(connection, dataTable, transaction);
            dataTable.Rows.Clear();
        }

        await ExecuteScript("Scripts/ErpProjectValues_Finish.sql", connection, transaction);
    }

    private static async Task ExecuteScript(string fileName, SqlConnection connection, SqlTransaction transaction)
    {
        Log.Information("Executing script {FileName}", fileName);
        // TODO: use connection.CreateBatch() once it is implemented for SqlConnection
        foreach (var script in SqlHelper.GetBatchesFromFile(fileName))
        {
            using var command = connection.CreateCommand();
            command.CommandText = script;
            command.Transaction = transaction;
            await command.ExecuteNonQueryAsync();
        }
    }

    private static async Task SyncDataTable(SqlConnection connection, DataTable dataTable, SqlTransaction transaction)
    {
        using var bulkCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.Default, transaction);
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