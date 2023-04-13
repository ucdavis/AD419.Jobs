using System;
using Microsoft.Azure.Functions.Worker;
using Serilog;
using AD419Functions.Services;

namespace AD419Functions;

public class PullAggieEnterpriseData
{
    private readonly SyncService _syncService;

    public PullAggieEnterpriseData(SyncService syncService)
    {
        _syncService = syncService;
    }

    [Function("PullAggieEnterpriseData")]
    public async Task Run([TimerTrigger("%PullAggieEnterpriseDataSchedule%"
        #if DEBUG
            , RunOnStartup = true
        #endif
        )] MyInfo myTimer)
    {
        Log.Information($"C# Timer trigger function executed at: {DateTime.Now}");
        await _syncService.Run();
    }
}

public class MyInfo
{
    public MyScheduleStatus? ScheduleStatus { get; set; }

    public bool IsPastDue { get; set; }
}

public class MyScheduleStatus
{
    public DateTime Last { get; set; }

    public DateTime Next { get; set; }

    public DateTime LastUpdated { get; set; }
}

