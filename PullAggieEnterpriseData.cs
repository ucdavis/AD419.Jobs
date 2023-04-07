using System;
using Microsoft.Azure.Functions.Worker;
using Serilog;
using AD419Functions.Services;

namespace AD419Functions;

public class PullAggieEnterpriseData
{
    private readonly AggieEnterpriseService _aggieEnterpriseService;

    public PullAggieEnterpriseData(AggieEnterpriseService aggieEnterpriseService)
    {
        _aggieEnterpriseService = aggieEnterpriseService;
    }

    [Function("PullAggieEnterpriseData")]
    public async Task Run([TimerTrigger("%PullAggieEnterpriseDataSchedule%"
        #if DEBUG
            , RunOnStartup = true
        #endif
        )] MyInfo myTimer)
    {
        Log.Information($"C# Timer trigger function executed at: {DateTime.Now}");
        await _aggieEnterpriseService.Test();
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

