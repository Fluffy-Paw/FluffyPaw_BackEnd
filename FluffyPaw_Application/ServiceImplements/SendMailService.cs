using FluffyPaw_Application.DTO.Request.EmailRequest;
using FluffyPaw_Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Application.ServiceImplements
{
    public class SendMailService : ISendMailService
    {
        public SendMailService()
        {
        }

        public async Task<bool> SendEmail(SendMailRequest sendMailRequest)
        {
            try
            {
                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress("Fluffy Paw <fluffypaw4u@gmail.com>");
                    mail.To.Add(sendMailRequest.Email.Trim());
                    mail.Subject = sendMailRequest.Title;
                    mail.Body = "<body>" + sendMailRequest.Message + "</body>";
                    mail.IsBodyHtml = true;

                    using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                    {
                        smtp.UseDefaultCredentials = false;
                        smtp.Credentials = new System.Net.NetworkCredential("fluffypaw4u@gmail.com", "itwc ugdw oivd dsnd");
                        smtp.EnableSsl = true;
                        smtp.Send(mail);
                    }
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
