﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Application.DTO.Response.AuthResponse
{
    public class ForgetPasswordResponse
    {
        public string Email { get; set; }
        public string OTP { get; set; }
    }
}
