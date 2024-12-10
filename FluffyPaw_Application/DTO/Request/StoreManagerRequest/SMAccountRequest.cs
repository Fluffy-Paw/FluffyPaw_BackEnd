using FluffyPaw_Application.Mapper;
using FluffyPaw_Domain.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Application.DTO.Request.StoreManagerRequest
{
    public class SMAccountRequest : IMapFrom<Account>, IMapFrom<Identification>
    {
        public string? Username { get; set; }

        public string? Password { get; set; }

        public string? FullName { get; set; }

        public IFormFile? Avatar { get; set; }

        public string? Email { get; set; }
    }
}
