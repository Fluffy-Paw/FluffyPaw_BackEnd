﻿using FluffyPaw_Application.Mapper;
using FluffyPaw_Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Application.DTO.Request.AuthRequest
{
    public class LoginRequest : IMapFrom<Account>
    {
        public string Username { get; set; }

        public string Password { get; set; }
    }
}