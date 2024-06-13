using AD419.Jobs.Core.Services;
using AD419.Jobs.Core.Utilities;
using Serilog;

namespace AD419.Jobs.Core.Extensions;

public static class SqlDataContextExtensions
{
    public static async Task ExecuteScriptFromFile(this ISqlDataContext sqlDataContext, string fileName)
    {
        Log.Information("Executing script named {FileName}", Path.GetFileName(fileName));
        // TODO: use connection.CreateBatch() once it is implemented for SqlConnection
        foreach (var batch in SqlHelper.GetBatchesFromFile(fileName))
        {
            await sqlDataContext.ExecuteNonQuery(batch);
        }
    }

    public static async Task ExecuteScriptFromString(this ISqlDataContext sqlDataContext, string script)
    {
        Log.Information("Attempting to execute sql script: {SqlScript}", script);
        foreach (var batch in SqlHelper.GetBatchesFromString(script))
        {
            await sqlDataContext.ExecuteNonQuery(batch);
        }
    }
}
