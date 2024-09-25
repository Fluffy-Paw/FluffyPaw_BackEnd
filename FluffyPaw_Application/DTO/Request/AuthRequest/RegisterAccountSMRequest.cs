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
    public class RegisterAccountSMRequest : IMapFrom<Account>, IMapFrom<StoreManager>
    {
        [Required(ErrorMessage = "Vui lòng nhập Username.")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập Password.")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập confirm password.")]
        public string ComfirmPassword { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập tên.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập email.")]
        [EmailAddress(ErrorMessage = "Vui lòng nhập đúng định dạng email.")]
        public string Email { get; set; }
        [Required]
        public string Logo { get; set; }
        [Required]
        public string BusinessLicense { get; set; }
    }
}
