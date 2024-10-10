using AutoMapper;
using FluffyPaw_Application.DTO.Response.ServiceResponse;
using FluffyPaw_Application.Mapper;
using FluffyPaw_Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Application.DTO.Response.StoreManagerResponse
{
    public class StaffResponse : IMapFrom<Account>, IMapFrom<Store>
    {
        public long Id { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string? Avatar { get; set; }

        public string RoleName { get; set; }

        public string Email { get; set; }

        public DateTimeOffset CreateDate { get; set; }

        public int Status { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public string Phone { get; set; }

    }
}
