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
            return CustomResult("Register Success", user);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            string tuple = await _authService.Login(loginRequest);
            return CustomResult("Login Success", tuple);
        }
    }
}
