using FluffyPaw_Application.Services;
using FluffyPaw_Domain.Interfaces;
using FluffyPaw_Repository.Enum;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Infrastructure.Intergrations.Quartz
{
    public class OverTimeRefundNotificationJob : IJob
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly INotificationService _notificationService;

        public OverTimeRefundNotificationJob(IUnitOfWork unitOfWork, INotificationService notificationService)
        {
            _unitOfWork = unitOfWork;
            _notificationService = notificationService;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var bookingId = context.JobDetail.JobDataMap.GetLong("BookingId");
            var booking = _unitOfWork.BookingRepository.Get(b => b.Id == bookingId,
                                includeProperties: "Pet,Pet.PetOwner,Pet.PetOwner.Account," +
                                "StoreService,StoreService.Service,StoreService.Service.ServiceType").FirstOrDefault();
            if (booking.Status == BookingStatus.Pending.ToString())
            {
                var description = $"Thông báo dịch vụ {booking.StoreService.Service.Name} của {booking.Pet.Name} đã quá thời gian chờ." +
                                $" Hệ thống tự động hoàn tiền...";

                await _notificationService.ScheduleCreateNotification(
                    booking.Pet.PetOwner.Account.Id,
                    booking.StoreService.Service.Name,
                    booking.StoreService.Service.ServiceType.Name,
                    description,
                    booking.Id
                );

                booking.Status = BookingStatus.OverTime.ToString();
                _unitOfWork.BookingRepository.Update(booking);

                var pet = _unitOfWork.PetRepository.GetByID(booking.PetId);
                var po = _unitOfWork.PetOwnerRepository.GetByID(pet.PetOwnerId);
                var wallet = _unitOfWork.WalletRepository.GetByID(po.AccountId);
                wallet.Balance += booking.Cost;
                _unitOfWork.WalletRepository.Update(wallet);

                await _unitOfWork.SaveAsync();
                
            }
        }
    }
}
