using System.Data;
using AggieEnterpriseApi;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using AD419.Jobs.Configuration;
using Serilog;
using AD419.Jobs.Core.Utilities;
using AD419.Jobs.Core.Configuration;
using AD419.Jobs.Core.Services;
using AD419.Jobs.Core.Extensions;

namespace AD419.Jobs.Services;

public class SyncService
{
    private readonly AggieEnterpriseService _aggieEnterpriseService;
    ISqlDataContext _sqlDataContext;
    private readonly SyncOptions _syncOptions;
    private readonly DataTable _dataTable;

    public SyncService(AggieEnterpriseService aggieEnterpriseService, ISqlDataContext sqlDataContext, IOptions<SyncOptions> syncOptions)
    {
        _aggieEnterpriseService = aggieEnterpriseService;
        _sqlDataContext = sqlDataContext;
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
        await _sqlDataContext.BeginTransaction();
        try
        {
            await SyncFinancialDepartmentValues();
            await SyncFundValues();
            await SyncAccountValues();
            await SyncProjectValues();
            await _sqlDataContext.CommitTransaction();
        }
        catch (Exception e)
        {
            Log.Error(e, "Error syncing");
            try
            {
                await _sqlDataContext.RollbackTransaction();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error rolling back transaction");
            }
            throw;
        }

    }

    private async Task SyncFinancialDepartmentValues()
    {
        await _sqlDataContext.ExecuteScriptFromFile("Scripts/ErpFinancialDepartmentValues_Start.sql");

        var i = 0;
        await foreach (var item in _aggieEnterpriseService.GetFinancialDepartmentValues())
        {
            _dataTable.Add(item.Id, item.Code, item.Name, item.ParentCode);
            if (++i % _syncOptions.BulkCopyBatchSize == 0)
            {
                Log.Information("Syncing batch of {BatchSize} financial department values", _syncOptions.BulkCopyBatchSize);
                await _sqlDataContext.BulkCopy(_dataTable, "#ErpFinancialDepartmentValues", 0);
                _dataTable.Rows.Clear();
            }
        }

        if (_dataTable.Rows.Count > 0)
        {
            // one last batch
            Log.Information("Syncing batch of {BatchSize} financial department values", _dataTable.Rows.Count);
            await _sqlDataContext.BulkCopy(_dataTable, "#ErpFinancialDepartmentValues", 0);
            _dataTable.Rows.Clear();
        }

        await _sqlDataContext.ExecuteScriptFromFile("Scripts/ErpFinancialDepartmentValues_Finish.sql");
    }

    private async Task SyncFundValues()
    {
        await _sqlDataContext.ExecuteScriptFromFile("Scripts/ErpFundValues_Start.sql");

        var i = 0;
        await foreach (var item in _aggieEnterpriseService.GetFundValues())
        {
            _dataTable.Add(item.Id, item.Code, item.Name, item.ParentCode);
            if (++i % _syncOptions.BulkCopyBatchSize == 0)
            {
                Log.Information("Syncing batch of {BatchSize} fund values", _syncOptions.BulkCopyBatchSize);
                await _sqlDataContext.BulkCopy(_dataTable, "#ErpFundValues", 0);
                _dataTable.Rows.Clear();
            }
        }

        if (_dataTable.Rows.Count > 0)
        {
            // one last batch
            Log.Information("Syncing batch of {BatchSize} fund values", _dataTable.Rows.Count);
            await _sqlDataContext.BulkCopy(_dataTable, "#ErpFundValues", 0);
            _dataTable.Rows.Clear();
        }

        await _sqlDataContext.ExecuteScriptFromFile("Scripts/ErpFundValues_Finish.sql");
    }

    private async Task SyncAccountValues()
    {
        await _sqlDataContext.ExecuteScriptFromFile("Scripts/ErpAccountValues_Start.sql");

        var i = 0;
        await foreach (var item in _aggieEnterpriseService.GetAccountValues())
        {
            _dataTable.Add(item.Id, item.Code, item.Name, item.ParentCode);
            if (++i % _syncOptions.BulkCopyBatchSize == 0)
            {
                Log.Information("Syncing batch of {BatchSize} account values", _syncOptions.BulkCopyBatchSize);
                await _sqlDataContext.BulkCopy(_dataTable, "#ErpAccountValues", 0);
                _dataTable.Rows.Clear();
            }
        }

        if (_dataTable.Rows.Count > 0)
        {
            // one last batch
            Log.Information("Syncing batch of {BatchSize} account values", _dataTable.Rows.Count);
            await _sqlDataContext.BulkCopy(_dataTable, "#ErpAccountValues", 0);
            _dataTable.Rows.Clear();
        }

        await _sqlDataContext.ExecuteScriptFromFile("Scripts/ErpAccountValues_Finish.sql");
    }

    private async Task SyncProjectValues()
    {
        await _sqlDataContext.ExecuteScriptFromFile("Scripts/ErpProjectValues_Start.sql");

        var i = 0;
        await foreach (var item in _aggieEnterpriseService.GetProjectValues())
        {
            _dataTable.Add(item.Id, item.Code, item.Name, item.ParentCode);
            if (++i % _syncOptions.BulkCopyBatchSize == 0)
            {
                Log.Information("Syncing batch of {BatchSize} Project values", _syncOptions.BulkCopyBatchSize);
                await _sqlDataContext.BulkCopy(_dataTable, "#ErpProjectValues", 0);
                _dataTable.Rows.Clear();
            }
        }

        if (_dataTable.Rows.Count > 0)
        {
            // one last batch
            Log.Information("Syncing batch of {BatchSize} Project values", _dataTable.Rows.Count);
            await _sqlDataContext.BulkCopy(_dataTable, "#ErpProjectValues", 0);
            _dataTable.Rows.Clear();
        }

        await _sqlDataContext.ExecuteScriptFromFile("Scripts/ErpProjectValues_Finish.sql");
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