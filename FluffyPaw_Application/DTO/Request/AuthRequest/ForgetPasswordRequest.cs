using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Application.DTO.Request.AuthRequest
{
    public class ForgetPasswordRequest
    {
        [Required(ErrorMessage = "Vui lòng nhập Email.")]
        [EmailAddress(ErrorMessage = "Vui lòng nhập đúng định dạng email.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập mật khẩu mới.")]
        public string NewPassword { get; set; }
    }
}
