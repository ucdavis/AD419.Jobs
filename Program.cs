using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Exceptions;
using AD419Functions.Services;
using AD419Functions.Configuration;
using Serilog.Sinks.Elasticsearch;
using System.Diagnostics;

#if DEBUG
Serilog.Debugging.SelfLog.Enable(msg => Debug.WriteLine(msg));
#endif

var host = new HostBuilder()
    .ConfigureAppConfiguration((hostContext, builder) =>
     {
         if (hostContext.HostingEnvironment.IsDevelopment())
         {
             builder.AddUserSecrets<Program>();
         }
     })
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices((hostContext, services) =>
    {
        var loggingSection = hostContext.Configuration.GetSection("Serilog");

        var loggerConfig = new LoggerConfiguration()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .MinimumLevel.Override("Worker", LogEventLevel.Warning)
            .MinimumLevel.Override("Host", LogEventLevel.Warning)
            .MinimumLevel.Override("System", LogEventLevel.Error)
            .MinimumLevel.Override("Function", LogEventLevel.Error)
            .MinimumLevel.Override("Azure.Storage.Blobs", LogEventLevel.Error)
            .MinimumLevel.Override("Azure.Core", LogEventLevel.Error)
            .Enrich.WithExceptionDetails()
            .Enrich.WithProperty("Application", loggingSection.GetValue<string>("AppName"))
            .Enrich.WithProperty("AppEnvironment", loggingSection.GetValue<string>("Environment"))
            .Enrich.FromLogContext()
            .WriteTo.Console();

        // add in elastic search sink if the uri is valid
        if (Uri.TryCreate(loggingSection.GetValue<string>("ElasticUrl"), UriKind.Absolute, out var elasticUri))
        {
            loggerConfig = loggerConfig.WriteTo.Elasticsearch(new ElasticsearchSinkOptions(elasticUri)
            {
                IndexFormat = "aspnet-ad419-jobs-{0:yyyy.MM}",
                TypeName = null
            });
        }

        Log.Logger = loggerConfig.CreateLogger();

        services.AddLogging(builder =>
        {
            builder.AddSerilog(Log.Logger, dispose: true);
        });

        services.Configure<ConnectionStrings>(hostContext.Configuration.GetSection("ConnectionStrings"));
        services.Configure<AggieEnterpriseOptions>(hostContext.Configuration.GetSection("AggieEnterprise"));
        services.Configure<SyncOptions>(hostContext.Configuration.GetSection("SyncService"));

        services.AddSingleton<AggieEnterpriseService>();
        services.AddSingleton<SyncService>();
    })
    .Build();

try
{
    Log.Information("Starting host");
    host.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
