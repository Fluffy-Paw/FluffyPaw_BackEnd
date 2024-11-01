using FluffyPaw_Application.DTO.Request.EmailRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Application.Services
{
    public interface ISendMailService
    {
        Task<string> SendMailOtp(SendMailRequest sendMailRequest);
        Task<bool> SendReceipt(SendMailRequest sendMailRequest);
    }
}
