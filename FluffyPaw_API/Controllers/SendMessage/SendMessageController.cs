using CoreApiResponse;
using FluffyPaw_Application.DTO.Request.EmailRequest;
using FluffyPaw_Application.DTO.Request.VacineRequest;
using FluffyPaw_Application.ServiceImplements;
using FluffyPaw_Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FluffyPaw_API.Controllers.SendMessage
{
    [Route("api/[controller]")]
    [ApiController]
    public class SendMessageController : BaseController
    {
        private readonly ISendMailService _sendMailService;

        public SendMessageController(ISendMailService sendMailService)
        {
            _sendMailService = sendMailService;
        }

        [HttpPost("SendEmail")]
        public async Task<IActionResult> AddVaccine([FromBody] SendMailRequest sendMailRequest)
        {
            var result = await _sendMailService.SendEmail(sendMailRequest);
            if(result) return CustomResult("Gửi mail thành công.");
            else return CustomResult("Gửi mail thất bại.");
        }
    }
}
