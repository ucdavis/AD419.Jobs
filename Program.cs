using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Exceptions;

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

        Log.Logger = new LoggerConfiguration()
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
            .WriteTo.Console()
            .CreateLogger();

        services.AddLogging(builder =>
        {
            builder.AddSerilog(Log.Logger, dispose: true);
        });
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
