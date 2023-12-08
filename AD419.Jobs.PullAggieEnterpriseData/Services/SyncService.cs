using System.Data;
using AggieEnterpriseApi;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using AD419.Jobs.Configuration;
using Serilog;
using AD419.Jobs.Core.Utilities;

namespace AD419.Jobs.Services;

public class SyncService
{
    private readonly AggieEnterpriseService _aggieEnterpriseService;
    private readonly ConnectionStrings _connectionStrings;
    private readonly SyncOptions _syncOptions;
    private readonly DataTable _dataTable;

    public SyncService(AggieEnterpriseService aggieEnterpriseService, IOptions<ConnectionStrings> connectionStrings, IOptions<SyncOptions> syncOptions)
    {
        _aggieEnterpriseService = aggieEnterpriseService;
        _connectionStrings = connectionStrings.Value;
        _syncOptions = syncOptions.Value;
        _dataTable = new DataTable();
        _dataTable.Columns.Add("Id", typeof(long));
        _dataTable.Columns.Add("Code", typeof(string));
        _dataTable.Columns.Add("Name", typeof(string));
        _dataTable.Columns.Add("ParentCode", typeof(string));

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
        await SqlHelper.ExecuteScript("Scripts/ErpFinancialDepartmentValues_Start.sql", connection, transaction);

        var i = 0;
        await foreach (var item in _aggieEnterpriseService.GetFinancialDepartmentValues())
        {
            _dataTable.Add(item.Id, item.Code, item.Name, item.ParentCode);
            if (++i % _syncOptions.BulkCopyBatchSize == 0)
            {
                Log.Information("Syncing batch of {BatchSize} financial department values", _syncOptions.BulkCopyBatchSize);
                await SyncDataTable(connection, _dataTable, transaction, "#ErpFinancialDepartmentValues");
                _dataTable.Rows.Clear();
            }
        }

        if (_dataTable.Rows.Count > 0)
        {
            // one last batch
            Log.Information("Syncing batch of {BatchSize} financial department values", _dataTable.Rows.Count);
            await SyncDataTable(connection, _dataTable, transaction, "#ErpFinancialDepartmentValues");
            _dataTable.Rows.Clear();
        }

        await SqlHelper.ExecuteScript("Scripts/ErpFinancialDepartmentValues_Finish.sql", connection, transaction);
    }

    private async Task SyncFundValues(SqlConnection connection, SqlTransaction transaction)
    {
        await SqlHelper.ExecuteScript("Scripts/ErpFundValues_Start.sql", connection, transaction);

        var i = 0;
        await foreach (var item in _aggieEnterpriseService.GetFundValues())
        {
            _dataTable.Add(item.Id, item.Code, item.Name, item.ParentCode);
            if (++i % _syncOptions.BulkCopyBatchSize == 0)
            {
                Log.Information("Syncing batch of {BatchSize} fund values", _syncOptions.BulkCopyBatchSize);
                await SyncDataTable(connection, _dataTable, transaction, "#ErpFundValues");
                _dataTable.Rows.Clear();
            }
        }

        if (_dataTable.Rows.Count > 0)
        {
            // one last batch
            Log.Information("Syncing batch of {BatchSize} fund values", _dataTable.Rows.Count);
            await SyncDataTable(connection, _dataTable, transaction, "#ErpFundValues");
            _dataTable.Rows.Clear();
        }

        await SqlHelper.ExecuteScript("Scripts/ErpFundValues_Finish.sql", connection, transaction);
    }

    private async Task SyncAccountValues(SqlConnection connection, SqlTransaction transaction)
    {
        await SqlHelper.ExecuteScript("Scripts/ErpAccountValues_Start.sql", connection, transaction);

        var i = 0;
        await foreach (var item in _aggieEnterpriseService.GetAccountValues())
        {
            _dataTable.Add(item.Id, item.Code, item.Name, item.ParentCode);
            if (++i % _syncOptions.BulkCopyBatchSize == 0)
            {
                Log.Information("Syncing batch of {BatchSize} account values", _syncOptions.BulkCopyBatchSize);
                await SyncDataTable(connection, _dataTable, transaction, "#ErpAccountValues");
                _dataTable.Rows.Clear();
            }
        }

        if (_dataTable.Rows.Count > 0)
        {
            // one last batch
            Log.Information("Syncing batch of {BatchSize} account values", _dataTable.Rows.Count);
            await SyncDataTable(connection, _dataTable, transaction, "#ErpAccountValues");
            _dataTable.Rows.Clear();
        }

        await SqlHelper.ExecuteScript("Scripts/ErpAccountValues_Finish.sql", connection, transaction);
    }

    private async Task SyncProjectValues(SqlConnection connection, SqlTransaction transaction)
    {
        await SqlHelper.ExecuteScript("Scripts/ErpProjectValues_Start.sql", connection, transaction);

        var i = 0;
        await foreach (var item in _aggieEnterpriseService.GetProjectValues())
        {
            _dataTable.Add(item.Id, item.Code, item.Name, item.ParentCode);
            if (++i % _syncOptions.BulkCopyBatchSize == 0)
            {
                Log.Information("Syncing batch of {BatchSize} Project values", _syncOptions.BulkCopyBatchSize);
                await SyncDataTable(connection, _dataTable, transaction, "#ErpProjectValues");
                _dataTable.Rows.Clear();
            }
        }

        if (_dataTable.Rows.Count > 0)
        {
            // one last batch
            Log.Information("Syncing batch of {BatchSize} Project values", _dataTable.Rows.Count);
            await SyncDataTable(connection, _dataTable, transaction, "#ErpProjectValues");
            _dataTable.Rows.Clear();
        }

        await SqlHelper.ExecuteScript("Scripts/ErpProjectValues_Finish.sql", connection, transaction);
    }

    private static async Task SyncDataTable(SqlConnection connection, DataTable dataTable, SqlTransaction transaction, string tableName)
    {
        using var bulkCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.Default, transaction);
        bulkCopy.DestinationTableName = tableName;
        bulkCopy.BatchSize = dataTable.Rows.Count;
        await bulkCopy.WriteToServerAsync(dataTable);
    }

}

public static class DataTableExtensions
{
    public static void Add(this DataTable dataTable, long id, string code, string name, string? parentCode)
    {
        var row = dataTable.NewRow();
        row["Id"] = id;
        row["Code"] = code;
        row["Name"] = name;
        row["ParentCode"] = parentCode as object ?? DBNull.Value;
        dataTable.Rows.Add(row);
    }
}