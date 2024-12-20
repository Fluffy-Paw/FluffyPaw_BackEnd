﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Application.DTO.Request.AuthRequest
{
    public class UpdatePasswordRequest
    {
        [Required(ErrorMessage = "Vui lòng nhập mật khẩu cũ.")]
        public string OldPassword { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập mật khẩu mới.")]
        public string NewPassword { get; set; }
    }
}
