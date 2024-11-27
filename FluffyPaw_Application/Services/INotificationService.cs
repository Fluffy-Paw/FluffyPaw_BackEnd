using FluffyPaw_Application.DTO.Request.NotificationRequest;
using FluffyPaw_Application.DTO.Response.NotificationResponse;
using FluffyPaw_Application.Utils.Pagination;
using FluffyPaw_Domain.Entities;
using FluffyPaw_Domain.Interfaces;
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
        Task<NotificationResponse> ScheduleCreateNotification(long receiverId, string name,
                                                                string type, string description, long referenceId);
        Task<bool> DeleteNotification(long notificationId);
        Task<bool> ChangeNotificationStatus();
        Task<IPaginatedList<Notification>> GetNotifications(int numberNoti);
    }
}
