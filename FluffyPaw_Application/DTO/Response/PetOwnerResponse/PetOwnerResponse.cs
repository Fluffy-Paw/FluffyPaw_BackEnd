﻿using FluffyPaw_Application.Mapper;
using FluffyPaw_Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Application.DTO.Response.PetOwnerResponse
{
    public class PetOwnerResponse : IMapFrom<Account>, IMapFrom<PetOwner>
    {
        public long Id { get; set; }

        public long AccountId { get; set; }

        public string FullName { get; set; }

        public string Gender { get; set; }

        public DateTimeOffset Dob { get; set; }

        public string Phone { get; set; }

        public string Address { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string? Avatar { get; set; }
    }
}
