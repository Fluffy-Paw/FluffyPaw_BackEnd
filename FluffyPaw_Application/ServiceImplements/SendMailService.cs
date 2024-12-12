using FluffyPaw_Application.DTO.Request.EmailRequest;
using FluffyPaw_Application.DTO.Response.AuthResponse;
using FluffyPaw_Application.Services;
using FluffyPaw_Domain.CustomException;
using FluffyPaw_Domain.Interfaces;
using FluffyPaw_Domain.Utils;
using Microsoft.AspNetCore.Authentication;
using System;
using System.Net.Mail;
using System.Text;

namespace FluffyPaw_Application.ServiceImplements
{
    public class SendMailService : ISendMailService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SendMailService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<string> SendOtpRegister(SendMailRequest sendMailRequest)
        {
            try
            {
                string otp = "";
                for (int i = 0; i < 6; i++)
                {
                    Random random = new Random();
                    otp += random.Next(0, 10).ToString();
                }
                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress("Fluffy Paw <fluffypaw4u@gmail.com>");
                    mail.To.Add(sendMailRequest.Email.Trim());
                    mail.Subject = "Fluffy Paw Account Verification";
                    mail.Body = "<body style=\"font-family: Arial, sans-serif; background-color: #f8e5f6; margin: 0; padding: 0;\">" +
                                    "<div style=\" max-width: 600px; margin: 20px auto; background-color: white; padding: 20px; border-radius: 8px; box-shadow: 0 0 10px rgba(0, 0, 0, 0.1); \">" +
                                        "<div style=\" background-color: #ffffff; color: white; text-align: center; padding: 20px; border-radius: 8px 8px 0 0; \">" +
                                            "<img src=\"https://firebasestorage.googleapis.com/v0/b/fluffy-paw-8e7c1.appspot.com/o/images%2FLogo.png?alt=media&token=44e03e8f-2630-4bbb-8f30-948ce9e5d7ce\" alt=\"Logo\" style=\"width: 280px; margin-bottom: 10px\"/>" +
                                        "</div>" +
                                        "<div style=\"padding: 20px; text-align: center; border-top: 1px solid #ddd\">" +
                                            "<p style=\"font-size: 16px; color: #333; text-align: left\">Xin chào,</p>" +
                                            "<p style=\"font-size: 16px; color: #333; text-align: left\">Đây là mã bảo mật tạm thời cho Tài khoản Fluffy Paw của bạn. Nó chỉ có thể được sử dụng một lần trong vòng <strong>5 phút</strong> tới, sau đó sẽ hết hạn: </p>" +
                                            "<div style=\" background-color: #f36fdd; color: white; font-size: 36px; font-weight: bold; padding: 20px; border-radius: 8px; margin: 20px 0; \">" + otp + "</div>" +
                                                "<p style=\"font-size: 16px; color: #333; text-align: left\">Bạn nhận được email này mà không có yêu cầu nhập mã xác thực nào từ Fluffy Paw? Nếu vậy, bảo mật tài khoản Fluffy Paw của bạn có thể đã bị xâm phạm. Vui lòng thay đổi mật khẩu của bạn càng sớm càng tốt.</p>" +
                                            "</div>" +
                                            "<div style=\" font-size: 14px; color: #666; text-align: center; padding: 10px; border-top: 1px solid #ddd;\">" +
                                            "<p style=\" margin: 0; text-align: left; padding-bottom: 10px; font-weight: 600; \"> Trân trọng, Fluffy Team </p>" +
                                            "<p style= margin: 0; text-align: left; font-weight: 600; color: rgb(182, 180, 180); \"> Đây là email được tạo tự động, vui lòng không trả lời. </p>" +
                                        "</div>" +
                                    "</div>" +
                                "</body>";
                    mail.IsBodyHtml = true;

                    using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                    {
                        smtp.UseDefaultCredentials = false;
                        smtp.Credentials = new System.Net.NetworkCredential("fluffypaw4u@gmail.com", "itwc ugdw oivd dsnd");
                        smtp.EnableSsl = true;
                        smtp.Send(mail);
                    }
                }

                return otp;
            }
            catch
            {
                throw new Exception();
            }
        }

        public async Task<ForgetPasswordResponse> SendOtpForgotPassword(SendMailPasswordRequest sendMailRequest) 
        {
            var user = _unitOfWork.AccountRepository.Get(a => a.Email == sendMailRequest.Email).FirstOrDefault();
            if (user == null) throw new CustomException.DataNotFoundException("Không tìm thấy tài khoản này. Hãy kiểm tra lại email của bạn.");

            try
            {
                string otp = "";
                for (int i = 0; i < 6; i++)
                {
                    Random random = new Random();
                    otp += random.Next(0, 10).ToString();
                }
                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress("Fluffy Paw <fluffypaw4u@gmail.com>");
                    mail.To.Add(user.Email);
                    mail.Subject = "Fluffy Paw Account Verification";
                    mail.Body = "<body style=\"font-family: Arial, sans-serif; background-color: #f8e5f6; margin: 0; padding: 0;\">" +
                                    "<div style=\" max-width: 600px; margin: 20px auto; background-color: white; padding: 20px; border-radius: 8px; box-shadow: 0 0 10px rgba(0, 0, 0, 0.1); \">" +
                                        "<div style=\" background-color: #ffffff; color: white; text-align: center; padding: 20px; border-radius: 8px 8px 0 0; \">" +
                                            "<img src=\"https://firebasestorage.googleapis.com/v0/b/fluffy-paw-8e7c1.appspot.com/o/images%2FLogo.png?alt=media&token=44e03e8f-2630-4bbb-8f30-948ce9e5d7ce\" alt=\"Logo\" style=\"width: 280px; margin-bottom: 10px\"/>" +
                                        "</div>" +
                                        "<div style=\"padding: 20px; text-align: center; border-top: 1px solid #ddd\">" +
                                            "<p style=\"font-size: 16px; color: #333; text-align: left\">Xin chào,</p>" +
                                            "<p style=\"font-size: 16px; color: #333; text-align: left\">Đây là mã bảo mật tạm thời cho Tài khoản Fluffy Paw của bạn. Nó chỉ có thể được sử dụng một lần trong vòng <strong>5 phút</strong> tới, sau đó sẽ hết hạn: </p>" +
                                            "<div style=\" background-color: #f36fdd; color: white; font-size: 36px; font-weight: bold; padding: 20px; border-radius: 8px; margin: 20px 0; \">" + otp + "</div>" +
                                                "<p style=\"font-size: 16px; color: #333; text-align: left\">Bạn nhận được email này mà không có yêu cầu nhập mã xác thực nào từ Fluffy Paw? Nếu vậy, bảo mật tài khoản Fluffy Paw của bạn có thể đã bị xâm phạm. Vui lòng thay đổi mật khẩu của bạn càng sớm càng tốt.</p>" +
                                            "</div>" +
                                            "<div style=\" font-size: 14px; color: #666; text-align: center; padding: 10px; border-top: 1px solid #ddd;\">" +
                                            "<p style=\" margin: 0; text-align: left; padding-bottom: 10px; font-weight: 600; \"> Trân trọng, Fluffy Team </p>" +
                                            "<p style= margin: 0; text-align: left; font-weight: 600; color: rgb(182, 180, 180); \"> Đây là email được tạo tự động, vui lòng không trả lời. </p>" +
                                        "</div>" +
                                    "</div>" +
                                "</body>";
                    mail.IsBodyHtml = true;

                    using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                    {
                        smtp.UseDefaultCredentials = false;
                        smtp.Credentials = new System.Net.NetworkCredential("fluffypaw4u@gmail.com", "itwc ugdw oivd dsnd");
                        smtp.EnableSsl = true;
                        smtp.Send(mail);
                    }
                }

                return new ForgetPasswordResponse { Email = user.Email, OTP = otp };
            }
            catch
            {
                throw new Exception();
            }
        }

        public async Task<bool> SendReceipt(SendReceiptRequest sendMailRequest)
        {
            try
            {
                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress("Fluffy Paw <fluffypaw4u@gmail.com>");
                    mail.To.Add(sendMailRequest.Email.Trim());
                    mail.Subject = "Fluffy Paw receipt";

                    StringBuilder bookingDetailsRows = new StringBuilder();
                    foreach (var booking in sendMailRequest.bookingResponses)
                    {
                        bookingDetailsRows.AppendLine(
                            "<tr>" +
                            $"<td style = \"padding: 10px 0\" > { booking.ServiceName } </td>" +
                            $"<td style = \"text-align: right; padding: 10px 0\"> { booking.Cost } VNĐ </td>" +
                            "</tr>");
                    }

                    mail.Body = "<body style = \"font-family: Arial, sans-serif; line-height: 1.6; margin: 0; padding: 0; background-color: #f8e5f6;\" >" +
                                    "<div style = \"max-width: 600px; margin: 20px auto; background-color: #ffffff; padding: 20px; border-radius: 5px; box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);\" >" +

                                    "<div style = \"text-align: center; margin-bottom: 20px\" >" +
                                        "<img alt = \"Logo\" src = \"https://firebasestorage.googleapis.com/v0/b/fluffy-paw-8e7c1.appspot.com/o/images%2FLogo.png?alt=media&token=44e03e8f-2630-4bbb-8f30-948ce9e5d7ce\" style = \"width: 260px\" />" +
                                    "</div>" +

                                    $"<h2 style = \"margin-bottom: 20px\" > Xin chào { sendMailRequest.CustomerName}, </h2>" +
                                    "<p style = \"font-size: 16px\" > Cảm ơn vì đã tin tưởng và sử dụng dịch vụ bên Fluffy Paw, hệ thống xin gửi đến cho bạn hóa đơn sau khi hoàn thành thanh toán dịch vụ. </p >" +

                                    "<div style = \"background-color: #f8f9fa; border-radius: 5px; padding: 15px; margin-bottom: 20px;\">" +
                                        $"<p style = \"margin: 0\" ><strong> Tổng số tiền thanh toán: { sendMailRequest.bookingResponses.Sum(b => b.Cost) } </strong></p >" +
                                        $"<p style = \"margin: 0\" > Ngày tạo đơn: { CoreHelper.SystemTimeNow.AddHours(7).ToString("dd-MM-yyyy") } </p >" +
                                    "</div >" +

                                    "<table style = \"width: 100%; border-collapse: collapse; margin-top: 20px\">" +
                                        "<tr style = \"border-bottom: 1px solid #eee\">" +
                                            "<th style = \"text-align: left; padding: 10px 0\" > Chi tiết </th>" +
                                            "<th style = \"text-align: right; padding: 10px 0\" > Số tiền </th>" +
                                        "</tr>" +

                                        bookingDetailsRows.ToString() +

                                        "<tr style = \"border-top: 1px solid #eee\" >" +
                                            "<td style = \"padding: 10px 0\" ><strong> Tổng cộng </ strong ></td>" +
                                            $"<td style = \"text-align: right; padding: 10px 0\"> <strong> { sendMailRequest.bookingResponses.Sum(b => b.Cost) } </strong> </td>" +
                                        "</tr>" +
                                    "</table>" +

                                    "<p style = \"margin-top: 20p\"> Một lần nữa Fluffy Paw chân thành cảm ơn quý khách đã trải nghiệm và tin tưởng.Nếu bạn cần hỗ trợ hãy liên hệ với quản lí qua địa chỉ gmail dưới đây để được hỗ trợ: </p>" +
                                    "<p style = \"text-decoration: dashed\"> fluffypaw4u@gmail.com </p>" +
                                    "<p> Thân ái,<br/> Fluffy Team </p>" +
                                    "<p style = \"margin: 0; text-align: left; font-weight: 600; color: rgb(182, 180, 180);\" > Đây là mail tự động, vui lòng không phản hồi lại. </p>" +
                                    "</div>" +
                                "</body>";
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

        public async Task<bool> SendBanMessage(SendMailRequest sendMailRequest)
        {
            try
            {
                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress("Fluffy Paw <fluffypaw4u@gmail.com>");
                    mail.To.Add(sendMailRequest.Email.Trim());
                    mail.Subject = "Fluffy Paw Account Verification";
                    mail.Body = "<body style=\"font-family: Arial, sans-serif; background-color: #f8e5f6; margin: 0; padding: 0;\">" +
                                    "<div style=\" max-width: 600px; margin: 20px auto; background-color: white; padding: 20px; border-radius: 8px; box-shadow: 0 0 10px rgba(0, 0, 0, 0.1); \">" +
                                        "<div style=\" background-color: #ffffff; color: white; text-align: center; padding: 20px; border-radius: 8px 8px 0 0; \">" +
                                            "<img src=\"https://firebasestorage.googleapis.com/v0/b/fluffy-paw-8e7c1.appspot.com/o/images%2FLogo.png?alt=media&token=44e03e8f-2630-4bbb-8f30-948ce9e5d7ce\" alt=\"Logo\" style=\"width: 280px; margin-bottom: 10px\"/>" +
                                        "</div>" +
                                        "<div style=\"padding: 20px; text-align: center; border-top: 1px solid #ddd\">" +
                                            "<p style=\"font-size: 16px; color: #333; text-align: left\">Xin chào,</p>" +
                                            "<p style=\"font-size: 16px; color: #333; text-align: left\">Sau khi xem xét, bạn đã bị hệ thống vô hiệu hóa tài khoản.</p>" +
                                            "<div style=\" background-color: red; color: white; font-size: 36px; font-weight: bold; padding: 20px; border-radius: 8px; margin: 20px 0; \">" + "Đã bị cấm" + "</div>" +
                                                "<p style=\"font-size: 16px; color: #333; text-align: left\">Nếu có khiếu nại, hãy liên hệ qua gmail: <strong>khoandse161173@fpt.edu.vn</strong></p>" +
                                            "</div>" +
                                            "<div style=\" font-size: 14px; color: #666; text-align: center; padding: 10px; border-top: 1px solid #ddd;\">" +
                                            "<p style=\" margin: 0; text-align: left; padding-bottom: 10px; font-weight: 600; \"> Trân trọng, Fluffy Team </p>" +
                                            "<p style= margin: 0; text-align: left; font-weight: 600; color: rgb(182, 180, 180); \"> Đây là email được tạo tự động, vui lòng không trả lời. </p>" +
                                        "</div>" +
                                    "</div>" +
                                "</body>";
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
                throw new Exception();
            }
        }
    }
}
