using FluffyPaw_Application.Services;
using FluffyPaw_Domain.Entities;
using FluffyPaw_Domain.Interfaces;
using FluffyPaw_Domain.Utils;
using FluffyPaw_Repository.Enum;
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
        private readonly INotificationService _notificationService;

        public BookingStartTimeNotificationJob(IUnitOfWork unitOfWork, INotificationService notificationService)
        {
            _unitOfWork = unitOfWork;
            _notificationService = notificationService;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var bookingId = context.JobDetail.JobDataMap.GetLong("BookingId");
            var booking = _unitOfWork.BookingRepository.Get(b => b.Id == bookingId
                                && b.Status == BookingStatus.Accepted.ToString(),
                                includeProperties: "Pet,Pet.PetOwner,Pet.PetOwner.Account," +
                                "StoreService,StoreService.Service,StoreService.Service.ServiceType").FirstOrDefault();

            var localBookingStartTime = booking.StartTime;
            var localCurrentTime = CoreHelper.SystemTimeNow;
            

            // Tính khoảng thời gian còn lại
            var timeRemaining = localBookingStartTime - CoreHelper.SystemTimeNow;
            if (timeRemaining.TotalSeconds < 0)
            {
                timeRemaining = timeRemaining.Negate(); // Đảo dấu TimeSpan
            }

            // Lấy tổng số giờ, phút, giây từ TimeSpan
            var totalHours = (int)timeRemaining.TotalHours; // Lấy tổng số giờ (làm tròn xuống)
            var totalMinutes = (int)(timeRemaining.TotalMinutes % 60); // Phần phút còn lại
            var totalSeconds = (int)(timeRemaining.TotalSeconds % 60); // Phần giây còn lại

            // Format chuỗi mô tả
            var description = $"Thông báo dịch vụ {booking.StoreService.Service.Name} " +
                              $"còn {totalHours} giờ {totalMinutes} phút {totalSeconds} giây sẽ bắt đầu.";
            Console.WriteLine(description);
            await _notificationService.ScheduleCreateNotification(booking.Pet.PetOwner.Account.Id,
                                            booking.StoreService.Service.Name, booking.StoreService.Service.ServiceType.Name,
                                            description, booking.Id);
        }
    }
}
