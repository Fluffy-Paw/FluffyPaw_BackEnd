using FluffyPaw_Application.DTO.Request.EmailRequest;
using FluffyPaw_Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Application.ServiceImplements
{
    public class SendMailService : ISendMailService
    {
        public SendMailService()
        {
        }

        public Task<bool> SendEmail(SendMailRequest sendMailRequest)
        {
            throw new NotImplementedException();
        }
    }
}
