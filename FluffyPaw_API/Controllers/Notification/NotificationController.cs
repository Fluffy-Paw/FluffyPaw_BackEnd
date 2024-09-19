using CoreApiResponse;
using FluffyPaw_Application.DTO.Request.NotificationRequest;
using FluffyPaw_Application.ServiceImplements;
using FluffyPaw_Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace FluffyPaw_API.Controllers.Notification
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : BaseController
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpGet("Get Noti")]
        public async Task<IActionResult> GetAllNotification(long receiverId)
        {
            var noti = await _notificationService.GetNotifications(receiverId);
            return CustomResult("Get noti Success", noti);
        }

        [HttpPost("Create Noti")]
        public async Task<IActionResult> CreateNotification([FromBody] NotificationRequest notiRequest)
        {
            var noti = await _notificationService.CreateNotification(notiRequest);
            return CustomResult("Create noti Success", noti);
        }

        [HttpDelete("Delete Noti")]
        public async Task<IActionResult> DeleteNotification(long notificationId)
        {
            var noti = await _notificationService.DeleteNotification(notificationId);
            return CustomResult("Delete noti Success", noti);
        }

        [HttpPost("Change Noti Status")]
        public async Task<IActionResult> ChangeNotificationStatus(long receiverId)
        {
            var noti = await _notificationService.ChangeNotificationStatus(receiverId);
            return CustomResult("All noti are readed", noti);
        }
    }
}
