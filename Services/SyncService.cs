using System.Collections.Concurrent;
using System.Data;
using AggieEnterpriseApi;
using AD419Functions.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using AD419Functions.Configuration;
using Serilog;
using System.Net;

namespace AD419Functions.Services;

public class SyncService
{
    private readonly AggieEnterpriseService _aggieEnterpriseService;
    private readonly ConcurrentDictionary<Type, DataTable> _dataTables = new();
    private readonly ConnectionStrings _connectionStrings;

    public SyncService(AggieEnterpriseService aggieEnterpriseService, IOptions<ConnectionStrings> connectionStrings)
    {
        _aggieEnterpriseService = aggieEnterpriseService;
        _connectionStrings = connectionStrings.Value;
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
        var dataTable = GetDataTable<IErpDepartmentSearch2_ErpFinancialDepartmentSearch_Data>("temp_ErpFinancialDepartmentValues");
        Log.Information("Creating table {TableName}", dataTable.TableName);
        await CreateDbTable(connection, dataTable);

        var batchSize = 1000;
        var i = 0;
        await foreach (var item in _aggieEnterpriseService.GetFinancialDepartmentValues(batchSize))
        {
            dataTable.Add(item);
            if (++i % batchSize == 0)
            {
                Log.Information("Syncing batch of {BatchSize} financial department values", batchSize);
                await SyncDataTable(connection, dataTable);
                dataTable.Rows.Clear();
                return;
            }
        }

        if (dataTable.Rows.Count > 0)
        {
            // one last batch
            await SyncDataTable(connection, dataTable);
            dataTable.Rows.Clear();
        }
    }

    private static async Task CreateDbTable<T>(SqlConnection connection, DataTable<T> dataTable)
    {
        // create table using the schema of the data table
        var sqlCommand = connection.CreateCommand();
        sqlCommand.CommandText = dataTable.GenerateDDL();
        await sqlCommand.ExecuteNonQueryAsync();
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