using AutoMapper;
using FluffyPaw_Application.DTO.Request.AuthRequest;
using FluffyPaw_Application.DTO.Request.NotificationRequest;
using FluffyPaw_Application.DTO.Response.NotificationResponse;
using FluffyPaw_Application.Services;
using FluffyPaw_Application.Utils.Pagination;
using FluffyPaw_Domain.CustomException;
using FluffyPaw_Domain.Entities;
using FluffyPaw_Domain.Enums;
using FluffyPaw_Domain.Interfaces;
using FluffyPaw_Domain.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
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
        private readonly ISignalRConfiguration _notiHub;
        private readonly IAuthentication _authentication;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public NotificationService(IUnitOfWork unitOfWork, IMapper mapper, ISignalRConfiguration notiHub, IAuthentication authentication, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _notiHub = notiHub;
            _authentication = authentication;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<bool> ChangeNotificationStatus()
        {
            var userId = _authentication.GetUserIdFromHttpContext(_httpContextAccessor.HttpContext);
            var ListNoti = _unitOfWork.NotificationRepository.Get(s => s.ReceiverId == userId && s.Status != NotificationStatus.Deleted.ToString());
            if (!ListNoti.Any())
            {
                throw new CustomException.DataNotFoundException("Bạn không có thông báo.");
            }
            foreach (var Notification in ListNoti)
            {
                Notification.Status = NotificationStatus.Readed.ToString();
                Notification.IsSeen = true;
                _unitOfWork.NotificationRepository.Update(Notification);
                _unitOfWork.Save();
            }
            return true;
        }

        public async Task<NotificationResponse> CreateNotification(NotificationRequest notificationRequest)
        {
            var existingUser = _unitOfWork.AccountRepository.Get(n => n.Id == notificationRequest.ReceiverId).FirstOrDefault();
            if (existingUser == null)
            {
                throw new CustomException.DataNotFoundException("Người dùng không tồn tại.");
            }

            var notification = _mapper.Map<Notification>(notificationRequest);
            notification.CreateDate = CoreHelper.SystemTimeNow;
            notification.IsSeen = false;
            notification.Status = NotificationStatus.Unread.ToString();
            _unitOfWork.NotificationRepository.Insert(notification);
            _unitOfWork.Save();

            await _notiHub.SendNotification(notification.Description, existingUser.Id);

            return _mapper.Map<NotificationResponse>(notification);
        }

        public async Task<NotificationResponse> ScheduleCreateNotification(long accountId, string name,
                                                                string type, string description)
        {
            var notification = new Notification
            {
                ReceiverId = accountId,
                Name = name,
                Type = type,
                Description = description,
                CreateDate = CoreHelper.SystemTimeNow,
                IsSeen = false,
                Status = NotificationStatus.Unread.ToString()
            };


            _unitOfWork.NotificationRepository.Insert(notification);
            _unitOfWork.Save();

            await _notiHub.SendNotification("ReceiveNoti", notification.ReceiverId);

            return _mapper.Map<NotificationResponse>(notification);
        }

        public async Task<bool> DeleteNotification(long notificationId)
        {
            var existingNoti = _unitOfWork.NotificationRepository.GetByID(notificationId) ?? throw new CustomException.DataNotFoundException("Thông báo không tồn tại.");

            existingNoti.Status = NotificationStatus.Deleted.ToString();
            _unitOfWork.NotificationRepository.Update(existingNoti);
            _unitOfWork.Save();

            return true;
        }

        public async Task<IPaginatedList<Notification>> GetNotifications(int numberNoti)
        {
            var userId = _authentication.GetUserIdFromHttpContext(_httpContextAccessor.HttpContext);
            var noti = _unitOfWork.NotificationRepository.Get(s => s.ReceiverId == userId && s.Status != NotificationStatus.Deleted.ToString(),
                                                orderBy: ob => ob.OrderByDescending(o => o.CreateDate)).AsQueryable();

            if (!noti.Any())
            {
                throw new CustomException.DataNotFoundException("Bạn không có thông báo.");
            }

            var result = await _unitOfWork.NotificationRepository.GetPagging(noti, 1, numberNoti * 5);
            return result;
        }
    }
}
