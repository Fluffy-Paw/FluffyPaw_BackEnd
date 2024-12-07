﻿using FluffyPaw_Application.Mapper;
using FluffyPaw_Domain.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Application.DTO.Request.PetOwnerRequest
{
    public class PetOwnerRequest : IMapFrom<Account>, IMapFrom<PetOwner>
    {
        public string? FullName { get; set; }

        public string? Gender { get; set; }

        public string? Address { get; set; }

        public string? Email { get; set; }

        public DateTimeOffset? Dob {  get; set; }

        public IFormFile? Avatar { get; set; }
    }
}
