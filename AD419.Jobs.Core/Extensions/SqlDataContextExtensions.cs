using AD419.Jobs.Core.Services;
using AD419.Jobs.Core.Utilities;
using Serilog;

namespace AD419.Jobs.Core.Extensions;

public static class SqlDataContextExtensions
{
    public static async Task ExecuteScriptFromFile(this ISqlDataContext sqlDataContext, string fileName, int timeoutSeconds = 30)
    {
        Log.Information("Executing script named {FileName}", Path.GetFileName(fileName));
        // TODO: use connection.CreateBatch() once it is implemented for SqlConnection
        foreach (var batch in SqlHelper.GetBatchesFromFile(fileName))
        {
            Log.Information("Attempting to execute sql batch: {batch}", batch);
            await sqlDataContext.ExecuteNonQuery(batch, timeoutSeconds);
        }
    }

    public static async Task ExecuteScriptFromString(this ISqlDataContext sqlDataContext, string script, int timeoutSeconds = 30)
    {
        foreach (var batch in SqlHelper.GetBatchesFromString(script))
        {
            Log.Information("Attempting to execute sql batch: {batch}", batch);
            await sqlDataContext.ExecuteNonQuery(batch, timeoutSeconds);
        }
    }
}
