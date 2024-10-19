using FluffyPaw_Domain.Entities;
using FluffyPaw_Domain.Interfaces;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Infrastructure.Intergrations.Quartz
{
    public class JobScheduler : IJobScheduler
    {
        private readonly IScheduler _scheduler;

        public JobScheduler(IScheduler scheduler)
        {
            _scheduler = scheduler;
        }

        public async Task BookingStartTimeNotificationJob(Booking booking)
        {

        }

        public async Task ScheduleStoreServiceClose(StoreService storeService)
        {
            var threeMinutesBeforeClose = storeService.StartTime.AddMinutes(-30);

            var jobKey = new JobKey($"StoreServiceCloseNotificationJob-{storeService.Id}");
            var triggerKey = new TriggerKey($"StoreServiceCloseNotificationJob-{storeService.Id}");

            if (await _scheduler.CheckExists(jobKey))
            {
                await _scheduler.DeleteJob(jobKey);
            }

            var job = JobBuilder.Create<StoreServiceCloseNotificationJob>()
                .WithIdentity(jobKey)
                .UsingJobData("StoreServiceId", storeService.Id)
                .UsingJobData("Status", storeService.Status)
                .Build();

            var trigger = TriggerBuilder.Create()
                .WithIdentity(triggerKey)
                .StartAt(threeMinutesBeforeClose)
                .Build();

            await _scheduler.ScheduleJob(job,trigger);
        }
    }
}
