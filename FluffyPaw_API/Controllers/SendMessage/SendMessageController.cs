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

        [HttpPost("SendOtpRegister")]
        public async Task<IActionResult> SendOtpRegister([FromBody] SendMailRequest sendMailRequest)
        {
            var otp = await _sendMailService.SendOtpRegister(sendMailRequest);
            return CustomResult("Gửi mail thành công, otp: ", otp);
        }

        [HttpPost("SendOtpForgotPassword")]
        public async Task<IActionResult> SendOtpForgotPassword([FromBody] SendMailPasswordRequest sendMailRequest)
        {
            var otp = await _sendMailService.SendOtpForgotPassword(sendMailRequest);
            return CustomResult("Gửi mail thành công, otp: ", otp);
        }

        [HttpPost("SendReceipt")]
        public async Task<IActionResult> SendReceipt([FromBody] SendMailRequest sendMailRequest)
        {
            var result = await _sendMailService.SendReceipt(sendMailRequest);
            if (result) return CustomResult("Gửi mail thành công.");
            else return CustomResult("Gửi mail thất bại.");
        }
    }
}
