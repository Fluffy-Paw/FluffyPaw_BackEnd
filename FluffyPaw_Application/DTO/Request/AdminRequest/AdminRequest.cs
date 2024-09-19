using FluffyPaw_Application.Mapper;
using FluffyPaw_Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Application.DTO.Request.AdminRequest
{
    public class AdminRequest : IMapFrom<Account>
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ComfirmPassword { get; set; }
        public string Email { get; set; }
    }
}
