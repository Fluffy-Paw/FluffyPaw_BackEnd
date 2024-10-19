using FluffyPaw_Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Domain.Interfaces
{
    public interface IJobScheduler
    {
        Task BookingStartTimeNotificationJob(Booking booking);
        Task ScheduleStoreServiceClose(StoreService storeService);
    }
}
