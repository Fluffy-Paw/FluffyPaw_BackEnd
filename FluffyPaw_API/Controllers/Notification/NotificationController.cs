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
        public async Task<IActionResult> GetAllNotification(long userId, int numberNoti)
        {
            var noti = await _notificationService.GetNotifications(userId, numberNoti);
            return CustomResult("Thông báo của bạn:", noti);
        }

        [HttpPost("Create Noti")]
        public async Task<IActionResult> CreateNotification([FromBody] NotificationRequest notiRequest)
        {
            var noti = await _notificationService.CreateNotification(notiRequest);
            return CustomResult("Tạo thông báo thành công.", noti);
        }

        [HttpDelete("Delete Noti")]
        public async Task<IActionResult> DeleteNotification(long notificationId)
        {
            var noti = await _notificationService.DeleteNotification(notificationId);
            return CustomResult("Xóa thông báo thành công.", noti);
        }

        [HttpPatch("Change Noti Status")]
        public async Task<IActionResult> ChangeNotificationStatus(long receiverId)
        {
            var noti = await _notificationService.ChangeNotificationStatus(receiverId);
            return CustomResult("Toàn bộ thông báo đã được đọc.", noti);
        }
    }
}
