using CoreApiResponse;
using FluffyPaw_Application.DTO.Request.AuthRequest;
using FluffyPaw_Application.DTO.Response.AuthResponse;
using FluffyPaw_Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FluffyPaw_API.Controllers.Authentication
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : BaseController
    {
        private readonly IAuthService _authService;

        public AuthenticationController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("RegisterPO")]
        public async Task<IActionResult> RegisterPO([FromBody] RegisterAccountPORequest registerAccountPORequest)
        {
            var user = await _authService.RegisterPO(registerAccountPORequest);
            return CustomResult("Đăng ký thành công.", user);
        }

        [HttpPost("RegisterSM")]
        public async Task<IActionResult> RegisterSM([FromForm] RegisterAccountSMRequest registerAccountSMRequest)
        {
            var user = await _authService.RegisterSM(registerAccountSMRequest);
            return CustomResult("Đăng ký thành công. Vui lòng đợi hệ thống xác thực thông tin đăng ký.", user);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            string tuple = await _authService.Login(loginRequest);
            return CustomResult("Đăng nhập thành công", tuple);
        }
    }
}
