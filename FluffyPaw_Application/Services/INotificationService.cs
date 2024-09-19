using FluffyPaw_Application.DTO.Request.NotificationRequest;
using FluffyPaw_Application.DTO.Response.NotificationResponse;
using FluffyPaw_Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Application.Services
{
    public interface INotificationService
    {
        Task<NotificationResponse> CreateNotification(NotificationRequest notificationRequest);
        Task<bool> DeleteNotification(long notificationId);
        Task<bool> ChangeNotificationStatus(long receiverId);
        Task<IEnumerable<NotificationResponse>> GetNotifications(long receiverId);
    }
}
