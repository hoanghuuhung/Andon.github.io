using Quartz;
using Quartz.Impl;
using System;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;

namespace AddOn.Utils
{
    public class Scheduler
    {
        public async void Start()

        {

            StdSchedulerFactory factory = new StdSchedulerFactory();

            // get a scheduler
            IScheduler scheduler = await factory.GetScheduler();
            await scheduler.Start();

            IJobDetail job = JobBuilder.Create<ExportExcelJob>()
     .WithIdentity("myJob", "group1")
     .Build();

            // Trigger the job to run now, and then every 40 seconds
            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("myTrigger", "group1")
                .WithSchedule(CronScheduleBuilder.DailyAtHourAndMinute(00,03) )
    .ForJob(job)
    .Build();

            await scheduler.ScheduleJob(job, trigger);

        }
   
    }
}
