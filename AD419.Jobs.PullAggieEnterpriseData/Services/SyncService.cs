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
    private readonly ISqlDataContext _sqlDataContext;
    private readonly SyncOptions _syncOptions;
    private readonly DataTable _heirarchicalDataTable;
    private readonly DataTable _dataTable;

    public SyncService(AggieEnterpriseService aggieEnterpriseService, ISqlDataContext sqlDataContext, IOptions<SyncOptions> syncOptions)
    {
        _aggieEnterpriseService = aggieEnterpriseService;
        _sqlDataContext = sqlDataContext;
        _syncOptions = syncOptions.Value;

        // TODO: Use just one datatable, rebuilding columns each time we need a different set of columns
        _heirarchicalDataTable = new DataTable();
        _heirarchicalDataTable.Columns.Add("Id", typeof(long));
        _heirarchicalDataTable.Columns.Add("Code", typeof(string));
        _heirarchicalDataTable.Columns.Add("Name", typeof(string));
        _heirarchicalDataTable.Columns.Add("ParentCode", typeof(string));

        _dataTable = new DataTable();
        _dataTable.Columns.Add("Id", typeof(long));
        _dataTable.Columns.Add("Code", typeof(string));
        _dataTable.Columns.Add("Name", typeof(string));
        _dataTable.Columns.Add("EligibleForUse", typeof(bool));

    }

    public async Task Run()
    {
        Log.Information("Starting sync");
        await _sqlDataContext.BeginTransaction();
        try
        {
            await SyncActivityValues();
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

    private async Task SyncActivityValues()
    {
        await _sqlDataContext.ExecuteScriptFromFile("Scripts/ErpActivityValues_Start.sql");

        var i = 0;
        await foreach (var item in _aggieEnterpriseService.GetActivityValues())
        {
            _dataTable.AddCode(item.Id, item.Code, item.Name, item.EligibleForUse);
            if (++i % _syncOptions.BulkCopyBatchSize == 0)
            {
                Log.Information("Syncing batch of {BatchSize} activity values", _syncOptions.BulkCopyBatchSize);
                await _sqlDataContext.BulkCopy(_dataTable, "#ErpActivityValues", 0);
                _dataTable.Rows.Clear();
            }
        }

        if (_dataTable.Rows.Count > 0)
        {
            // one last batch
            Log.Information("Syncing batch of {BatchSize} activity values", _dataTable.Rows.Count);
            await _sqlDataContext.BulkCopy(_dataTable, "#ErpActivityValues", 0);
            _dataTable.Rows.Clear();
        }

        await _sqlDataContext.ExecuteScriptFromFile("Scripts/ErpActivityValues_Finish.sql");
    }

    private async Task SyncFinancialDepartmentValues()
    {
        await _sqlDataContext.ExecuteScriptFromFile("Scripts/ErpFinancialDepartmentValues_Start.sql");

        var i = 0;
        await foreach (var item in _aggieEnterpriseService.GetFinancialDepartmentValues())
        {
            _heirarchicalDataTable.AddHeirarchicalCode(item.Id, item.Code, item.Name, item.ParentCode);
            if (++i % _syncOptions.BulkCopyBatchSize == 0)
            {
                Log.Information("Syncing batch of {BatchSize} financial department values", _syncOptions.BulkCopyBatchSize);
                await _sqlDataContext.BulkCopy(_heirarchicalDataTable, "#ErpFinancialDepartmentValues", 0);
                _heirarchicalDataTable.Rows.Clear();
            }
        }

        if (_heirarchicalDataTable.Rows.Count > 0)
        {
            // one last batch
            Log.Information("Syncing batch of {BatchSize} financial department values", _heirarchicalDataTable.Rows.Count);
            await _sqlDataContext.BulkCopy(_heirarchicalDataTable, "#ErpFinancialDepartmentValues", 0);
            _heirarchicalDataTable.Rows.Clear();
        }

        await _sqlDataContext.ExecuteScriptFromFile("Scripts/ErpFinancialDepartmentValues_Finish.sql");
    }

    private async Task SyncFundValues()
    {
        await _sqlDataContext.ExecuteScriptFromFile("Scripts/ErpFundValues_Start.sql");

        var i = 0;
        await foreach (var item in _aggieEnterpriseService.GetFundValues())
        {
            _heirarchicalDataTable.AddHeirarchicalCode(item.Id, item.Code, item.Name, item.ParentCode);
            if (++i % _syncOptions.BulkCopyBatchSize == 0)
            {
                Log.Information("Syncing batch of {BatchSize} fund values", _syncOptions.BulkCopyBatchSize);
                await _sqlDataContext.BulkCopy(_heirarchicalDataTable, "#ErpFundValues", 0);
                _heirarchicalDataTable.Rows.Clear();
            }
        }

        if (_heirarchicalDataTable.Rows.Count > 0)
        {
            // one last batch
            Log.Information("Syncing batch of {BatchSize} fund values", _heirarchicalDataTable.Rows.Count);
            await _sqlDataContext.BulkCopy(_heirarchicalDataTable, "#ErpFundValues", 0);
            _heirarchicalDataTable.Rows.Clear();
        }

        await _sqlDataContext.ExecuteScriptFromFile("Scripts/ErpFundValues_Finish.sql");
    }

    private async Task SyncAccountValues()
    {
        await _sqlDataContext.ExecuteScriptFromFile("Scripts/ErpAccountValues_Start.sql");

        var i = 0;
        await foreach (var item in _aggieEnterpriseService.GetAccountValues())
        {
            _heirarchicalDataTable.AddHeirarchicalCode(item.Id, item.Code, item.Name, item.ParentCode);
            if (++i % _syncOptions.BulkCopyBatchSize == 0)
            {
                Log.Information("Syncing batch of {BatchSize} account values", _syncOptions.BulkCopyBatchSize);
                await _sqlDataContext.BulkCopy(_heirarchicalDataTable, "#ErpAccountValues", 0);
                _heirarchicalDataTable.Rows.Clear();
            }
        }

        if (_heirarchicalDataTable.Rows.Count > 0)
        {
            // one last batch
            Log.Information("Syncing batch of {BatchSize} account values", _heirarchicalDataTable.Rows.Count);
            await _sqlDataContext.BulkCopy(_heirarchicalDataTable, "#ErpAccountValues", 0);
            _heirarchicalDataTable.Rows.Clear();
        }

        await _sqlDataContext.ExecuteScriptFromFile("Scripts/ErpAccountValues_Finish.sql");
    }

    private async Task SyncProjectValues()
    {
        await _sqlDataContext.ExecuteScriptFromFile("Scripts/ErpProjectValues_Start.sql");

        var i = 0;
        await foreach (var item in _aggieEnterpriseService.GetProjectValues())
        {
            _heirarchicalDataTable.AddHeirarchicalCode(item.Id, item.Code, item.Name, item.ParentCode);
            if (++i % _syncOptions.BulkCopyBatchSize == 0)
            {
                Log.Information("Syncing batch of {BatchSize} Project values", _syncOptions.BulkCopyBatchSize);
                await _sqlDataContext.BulkCopy(_heirarchicalDataTable, "#ErpProjectValues", 0);
                _heirarchicalDataTable.Rows.Clear();
            }
        }

        if (_heirarchicalDataTable.Rows.Count > 0)
        {
            // one last batch
            Log.Information("Syncing batch of {BatchSize} Project values", _heirarchicalDataTable.Rows.Count);
            await _sqlDataContext.BulkCopy(_heirarchicalDataTable, "#ErpProjectValues", 0);
            _heirarchicalDataTable.Rows.Clear();
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
    public static void AddHeirarchicalCode(this DataTable dataTable, long id, string code, string name, string? parentCode)
    {
        var row = dataTable.NewRow();
        row["Id"] = id;
        row["Code"] = code;
        row["Name"] = name;
        row["ParentCode"] = parentCode as object ?? DBNull.Value;
        dataTable.Rows.Add(row);
    }

    public static void AddCode(this DataTable dataTable, long id, string code, string name, bool eligibleForUse)
    {
        var row = dataTable.NewRow();
        row["Id"] = id;
        row["Code"] = code;
        row["Name"] = name;
        row["EligibleForUse"] = eligibleForUse as object ?? DBNull.Value;
        dataTable.Rows.Add(row);
    }

}