using FluffyPaw_Domain.Enums;
using FluffyPaw_Domain.Interfaces;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Infrastructure.Intergrations.Quartz
{
    public class StoreServiceCloseNotificationJob : IJob
    {
        private readonly IUnitOfWork _unitOfWork;

        public StoreServiceCloseNotificationJob(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var storeServiceId = context.JobDetail.JobDataMap.GetLong("StoreServiceId"));
            var storeService = _unitOfWork.StoreServiceRepository.GetByID(storeServiceId);

            if (storeService != null)
            {
                storeService.Status = StoreServiceStatus.NotAvailable.ToString();
                _unitOfWork.StoreServiceRepository.Update(storeService);
                await _unitOfWork.SaveAsync();
            }
        }
    }
}
