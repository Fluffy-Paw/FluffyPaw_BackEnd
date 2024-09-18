﻿using FluffyPaw_Application.Mapper;
using FluffyPaw_Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Application.DTO.Request.AuthRequest
{
    public class RegisterAccountSMRequest : IMapFrom<Account>, IMapFrom<StoreManager>
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ComfirmPassword { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Logo { get; set; }
        public string BusinessLicense { get; set; }
    }
}
