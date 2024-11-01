using CoreApiResponse;
using FluffyPaw_Application.DTO.Request.ConversationRequest;
using FluffyPaw_Application.DTO.Request.StoreServiceRequest;
using FluffyPaw_Application.ServiceImplements;
using FluffyPaw_Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FluffyPaw_API.Controllers.Conversation
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConversationController : BaseController
    {
        private readonly IConversationService _conversationService;

        public ConversationController(IConversationService conversationService)
        {
            _conversationService = conversationService;
        }

        [HttpGet("GetAllConversation")]
        [Authorize(Roles = "Staff,PetOwner")]
        public async Task<IActionResult> GetAllConversation()
        {
            var conversations = await _conversationService.GetAllConversation();
            return CustomResult("Lấy dữ liệu thành công", conversations);
        }

        [HttpPost("CreateConversation")]
        [Authorize(Roles = "Staff,PetOwner")]
        public async Task<IActionResult> CreateConversation([FromBody] ConversationRequest conversationRequest)
        {
            var conversation = await _conversationService.CreateConversation(conversationRequest);
            return CustomResult("Tạo hội thoại thành công", conversation);
        }

        [HttpPatch("OpenConversation/{id}")]
        [Authorize(Roles = "Staff,PetOwner")]
        public async Task<IActionResult> OpenConversation(long id)
        {
            var conversation = await _conversationService.OpenConversation(id);
            return CustomResult("Gỡ chặn hội thoại thành công.", conversation);
        }

        [HttpPatch("CloseConversation/{id}")]
        [Authorize(Roles = "Staff,PetOwner")]
        public async Task<IActionResult> CloseConversation(long id)
        {
            var conversation = await _conversationService.CloseConversation(id);
            return CustomResult("Chặn hội thoại thành công.", conversation);
        }

        [HttpPatch("DeleteConversation/{id}")]
        [Authorize(Roles = "Staff,PetOwner")]
        public async Task<IActionResult> DeleteConversation(long id)
        {
            var conversation = await _conversationService.DeleteConversation(id);
            return CustomResult("Xóa hội thoại thành công.", conversation);
        }

        [HttpGet("GetAllConversationMessageByConversationId/{id}")]
        [Authorize(Roles = "Staff,PetOwner")]
        public async Task<IActionResult> GetAllConversationMessageByConversationId(long id)
        {
            var conversationMessages = await _conversationService.GetAllConversationMessageByConversationId(id);
            return CustomResult("Lấy dữ liệu thành công", conversationMessages);
        }

        [HttpPost("SendMessage")]
        [Authorize(Roles = "Staff,PetOwner")]
        public async Task<IActionResult> SendMessage([FromForm] ConversationMessageRequest conversationMessageRequest)
        {
            var conversationMessage = await _conversationService.SendMessage(conversationMessageRequest);
            return CustomResult("Gửi tin nhắn thành công", conversationMessage);
        }
    }
}
