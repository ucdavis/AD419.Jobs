using AD419.Jobs.Configuration;
using AD419.Jobs.Core;
using AD419.Jobs.Services;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

class Program : JobBase
{
    private static ILogger? _log;
    static async Task Main(string[] args)
    {
        Configure();
        var assembyName = typeof(Program).Assembly.GetName();
        _log = Log.Logger
            .ForContext("jobname", assembyName.Name)
            .ForContext("jobid", Guid.NewGuid());

        _log.Information("Running {job} build {build}", assembyName.Name, assembyName.Version);
        var provider = ConfigureServices();

        var syncService = provider.GetRequiredService<SyncService>();

        await syncService.Run();   
     }

    private static ServiceProvider ConfigureServices()
    {
        IServiceCollection services = new ServiceCollection();

        services.Configure<ConnectionStrings>(Configuration.GetSection("ConnectionStrings"));
        services.Configure<AggieEnterpriseOptions>(Configuration.GetSection("AggieEnterprise"));
        services.Configure<SyncOptions>(Configuration.GetSection("SyncService"));

        services.AddSingleton<AggieEnterpriseService>();
        services.AddSingleton<SyncService>();

        return services.BuildServiceProvider();
    }
}