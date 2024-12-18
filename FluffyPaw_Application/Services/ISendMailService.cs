﻿using FluffyPaw_Application.DTO.Request.EmailRequest;
using FluffyPaw_Application.DTO.Response.AuthResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Application.Services
{
    public interface ISendMailService
    {
        Task<string> SendOtpRegister(SendMailRequest sendMailRequest);
        Task<ForgetPasswordResponse> SendOtpForgotPassword(SendMailRequest sendMailRequest);
        Task<bool> SendReceipt(SendReceiptRequest sendMailRequest);
        Task<bool> SendBanMessage(SendMailRequest sendMailRequest);
        Task<bool> SendDenyAccountMessage(SendMailDenyRequest sendMailRequest);
        Task<bool> SendAccountMessage(SendMailRequest sendMailRequest);
    }
}
