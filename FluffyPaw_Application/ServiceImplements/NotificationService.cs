using AutoMapper;
using FluffyPaw_Application.DTO.Request.AuthRequest;
using FluffyPaw_Application.DTO.Request.NotificationRequest;
using FluffyPaw_Application.DTO.Response.NotificationResponse;
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

        public async Task<bool> ChangeNotificationStatus(long receiverId)
        {
            var ListNoti = _unitOfWork.NotificationRepository.Get(n => n.ReceiverId == receiverId);
            if (!ListNoti.Any())
            {
                throw new CustomException.DataNotFoundException("Thông báo không tồn tại.");
            }
            foreach (var Notification in ListNoti)
            {
                Notification.Status = "Readed";
                Notification.IsSeen = true;
                _unitOfWork.NotificationRepository.Update(Notification);
                _unitOfWork.Save();
            }
            return true;
        }

        public async Task<NotificationResponse> CreateNotification(NotificationRequest notificationRequest)
        {
            var existingUser = _unitOfWork.AccountRepository.Get(n => n.Id == notificationRequest.ReceiverId);
            if (!existingUser.Any())
            {
                throw new CustomException.DataNotFoundException("Người dùng không tồn tại.");
            }

            var notification = _mapper.Map<Notification>(notificationRequest);
            notification.IsSeen = false;
            notification.Status = "Unread";
            notification.CreateDate = DateTime.Now;
            notification.StartTime = DateTime.Now;
            notification.EndTime = DateTime.Now;
            _unitOfWork.NotificationRepository.Insert(notification);
            _unitOfWork.Save();
            return _mapper.Map<NotificationResponse>(notification);
        }

        public async Task<bool> DeleteNotification(long notificationId)
        {
            var existingNoti = _unitOfWork.NotificationRepository.GetByID(notificationId);
            if (existingNoti == null)
            {
                throw new CustomException.DataNotFoundException("Thông báo không tồn tại.");
            }

            existingNoti.Status = "Deleted";
            _unitOfWork.NotificationRepository.Update(existingNoti);
            _unitOfWork.Save();

            return true;
        }

        public async Task<IEnumerable<NotificationResponse>> GetNotifications(long receiverId)
        {
            var noti = _unitOfWork.NotificationRepository.Get(s => s.ReceiverId == receiverId && s.Status != "Deleted");

            if ( noti.Any())
            {
                return _mapper.Map<IEnumerable<NotificationResponse>>(noti);
            }
            throw new CustomException.DataNotFoundException("Bạn không có thông báo.");
        }
    }
}
