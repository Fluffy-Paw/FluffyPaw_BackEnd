using FluffyPaw_Domain.Interfaces;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Infrastructure.Intergrations.Quartz
{
    public class BookingStartTimeNotificationJob : IJob
    {
        private readonly IUnitOfWork _unitOfWork;

        public BookingStartTimeNotificationJob(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Execute(IJobExecutionContext context)
        {

        }
    }
}
