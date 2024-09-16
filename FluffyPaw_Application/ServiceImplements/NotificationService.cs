using AutoMapper;
using FluffyPaw_Application.DTO.Request.AuthRequest;
using FluffyPaw_Application.DTO.Request.NotificationRequest;
using FluffyPaw_Application.Services;
using FluffyPaw_Domain.CustomException;
using FluffyPaw_Domain.Entities;
using FluffyPaw_Domain.Enums;
using FluffyPaw_Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Application.ServiceImplements
{
    public class NotificationService : INotificationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public NotificationService(IUnitOfWork unitOfWork, IMapper mapper, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _configuration = configuration;
        }

        public async Task<bool> ChangeNotificationStatus(long notificationId)
        {
            var existingNoti = _unitOfWork.NotificationRepository.GetByID(notificationId);
            if (existingNoti == null)
            {
                throw new CustomException.DataNotFoundException("Thông báo không tồn tại.");
            }

            existingNoti.Status = "Readed";
            existingNoti.IsSeen = true;
            _unitOfWork.NotificationRepository.Update(existingNoti);
            _unitOfWork.Save();

            return true;
        }

        public async Task<bool> CreateNotification(NotificationRequest notificationRequest)
        {
            var existingUser = _unitOfWork.AccountRepository.GetByID(notificationRequest.ReceiverId);
            if (existingUser == null)
            {
                throw new CustomException.DataNotFoundException("Người dùng không tồn tại.");
            }

            var notification = _mapper.Map<Notification>(notificationRequest);
            notification.IsSeen = false;
            notification.Status = "Unread";
            notification.CreateDate = DateTime.Now;
            _unitOfWork.NotificationRepository.Insert(notification);
            _unitOfWork.Save();
            return true;
        }

        public async Task<bool> DeleteNotification(long notificationId)
        {
            var existingNoti = _unitOfWork.NotificationRepository.GetByID(notificationId);
            if (existingNoti == null)
            {
                throw new CustomException.DataNotFoundException("Thông báo không tồn tại.");
            }

            _unitOfWork.NotificationRepository.Delete(existingNoti);
            _unitOfWork.Save();

            return true;
        }
    }
}
