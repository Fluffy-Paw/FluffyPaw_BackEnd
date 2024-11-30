using FluffyPaw_Domain.Entities;
using FluffyPaw_Domain.Interfaces;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
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

        public async Task ScheduleBookingNotification(Booking booking)
        {
            var jobKey = new JobKey($"BookingStartNotificationJob-{booking.Id}");
            var triggerKeyOneDay = new TriggerKey($"BookingStartNotificationTriggerOneDay-{booking.Id}");
            var triggerKeyOneHour = new TriggerKey($"BookingStartNotificationTriggerOneHour-{booking.Id}");

            if (await _scheduler.CheckExists(jobKey))
            {
                await _scheduler.DeleteJob(jobKey);
            }

            var job = JobBuilder.Create<BookingStartTimeNotificationJob>()
                .WithIdentity(jobKey)
                .UsingJobData("BookingId", booking.Id)
                .Build();

            var triggerOneDay = TriggerBuilder.Create()
                .WithIdentity(triggerKeyOneDay)
                .StartAt(booking.StartTime.AddMinutes(-5))
                .Build();

            var triggerOneHour = TriggerBuilder.Create()
                .WithIdentity(triggerKeyOneHour)
                .StartAt(booking.StartTime.AddMinutes(-1))
                .Build();

            await _scheduler.ScheduleJob(job, new HashSet<ITrigger> { triggerOneDay, triggerOneHour }, true);
        }

        public async Task ScheduleOverTimeRefund(Booking booking)
        {
            var jobKey = new JobKey($"OverTimeRefundNotificationJob-{booking.Id}");
            var triggerKeyOneHour = new TriggerKey($"OverTimeRefundNotificationOneHour-{booking.Id}");

            if (await _scheduler.CheckExists(jobKey))
            {
                await _scheduler.DeleteJob(jobKey);
            }

            var job = JobBuilder.Create<OverTimeRefundNotificationJob>()
                .WithIdentity(jobKey)
                .UsingJobData("BookingId", booking.Id)
                .Build();

            var triggerOneHour = TriggerBuilder.Create()
                .WithIdentity(triggerKeyOneHour)
                .StartAt(booking.CreateDate.AddMinutes(0.5))
            .Build();

            await _scheduler.ScheduleJob(job, triggerOneHour);
        }

        public async Task ScheduleStoreServiceClose(StoreService storeService)
        {
            var threeMinutesBeforeClose = storeService.StartTime.AddHours(-7.5);

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

            await _scheduler.ScheduleJob(job, trigger);
        }
        
    }
}
