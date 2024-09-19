using FluffyPaw_Application.DTO.Request.AuthRequest;
using FluffyPaw_Application.DTO.Response.AuthResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Application.Services
{
    public interface IAuthService
    {
        Task<bool> RegisterPO(RegisterAccountPORequest registerAccountPORequest);

        Task<bool> RegisterSM(RegisterAccountSMRequest registerAccountSMRequest);

        Task<string> Login(LoginRequest loginRequest);
    }
}
