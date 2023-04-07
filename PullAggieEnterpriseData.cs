using System;
using Microsoft.Azure.Functions.Worker;
using Serilog;

namespace AD419Functions
{
    public class PullAggieEnterpriseData
    {
        public PullAggieEnterpriseData()
        {
        }

        [Function("PullAggieEnterpriseData")]
        public void Run([TimerTrigger("%PullAggieEnterpriseDataSchedule%"
        #if DEBUG
            , RunOnStartup = true
        #endif
        )] MyInfo myTimer)
        {
            Log.Information($"C# Timer trigger function executed at: {DateTime.Now}");
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
}
