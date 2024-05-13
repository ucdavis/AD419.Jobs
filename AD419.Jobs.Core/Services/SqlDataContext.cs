using System.Data;
using AD419.Jobs.Core.Configuration;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using Serilog;

namespace AD419.Jobs.Core.Services;

public sealed class SqlDataContext : IDisposable, ISqlDataContext
{
    private SqlConnection? _sqlConnection;
    private SqlTransaction? _sqlTransaction;
    private readonly ConnectionStrings _connectionStrings;

    public SqlDataContext(IOptions<ConnectionStrings> connectionStrings)
    {
        _connectionStrings = connectionStrings.Value;
    }

    public void Dispose()
    {
        _sqlTransaction?.Dispose();
        _sqlConnection?.Dispose();
    }

    public async Task BeginTransaction()
    {
        if (_sqlTransaction == null)
        {
            var connection = await GetOpenConnection();
            _sqlTransaction = (SqlTransaction)await connection.BeginTransactionAsync();
        }
    }

    public async Task CommitTransaction()
    {
        if (_sqlTransaction == null)
        {
            throw new InvalidOperationException("Transaction has not been started.");
        }
        await _sqlTransaction.CommitAsync();
        _sqlTransaction.Dispose();
        _sqlTransaction = null;
    }

    public async Task RollbackTransaction()
    {
        if (_sqlTransaction == null)
        {
            throw new InvalidOperationException("Transaction has not been started.");
        }
        await _sqlTransaction.RollbackAsync();
        _sqlTransaction.Dispose();
        _sqlTransaction = null;
    }

    public async Task ExecuteNonQuery(string script)
    {
        using var connection = await GetOpenConnection();
        Log.Information("Executing batch: {BatchContent}", script);
        using var command = connection.CreateCommand();
        command.CommandText = script;
        command.Transaction = _sqlTransaction;
        await command.ExecuteNonQueryAsync();
    }

    public async Task BulkCopy(DataTable dataTable, string destinationTableName, int batchSize)
    {
        var connection = await GetOpenConnection();
        var bulkCopy = _sqlTransaction == null
            ? new SqlBulkCopy(connection)
            : new SqlBulkCopy(connection, SqlBulkCopyOptions.Default, _sqlTransaction);
        bulkCopy.DestinationTableName = destinationTableName;
        bulkCopy.BatchSize = batchSize;
        await bulkCopy.WriteToServerAsync(dataTable);
    }


    private async Task<SqlConnection> GetOpenConnection()
    {
        if (_sqlConnection == null)
        {
            _sqlConnection = new SqlConnection(_connectionStrings.DefaultConnection);
            await _sqlConnection.OpenAsync();
        }
        return _sqlConnection;
    }
}

public interface ISqlDataContext
{
    Task BeginTransaction();
    Task CommitTransaction();
    Task RollbackTransaction();
    Task ExecuteNonQuery(string script);
    Task BulkCopy(DataTable dataTable, string destinationTableName, int batchSize);
}