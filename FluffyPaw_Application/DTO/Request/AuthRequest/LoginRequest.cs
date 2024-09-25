using FluffyPaw_Application.Mapper;
using FluffyPaw_Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Application.DTO.Request.AuthRequest
{
    public class LoginRequest : IMapFrom<Account>
    {
        [Required(ErrorMessage = "Vui lòng nhập Username.")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập Password.")]
        public string Password { get; set; }
    }
}
