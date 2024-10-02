using FluffyPaw_Application.Mapper;
using FluffyPaw_Domain.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Application.DTO.Request.AuthRequest
{
    public class RegisterAccountSMRequest : IMapFrom<Account>, IMapFrom<Brand>, IMapFrom<Identification>
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
        public string Hotline { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập email.")]
        [EmailAddress(ErrorMessage = "Vui lòng nhập đúng định dạng email.")]
        public string BrandEmail { get; set; }

        [Required]
        public IFormFile Logo { get; set; }

        [Required]
        public IFormFile BusinessLicense { get; set; }

        [Required]
        public string MST { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        public IFormFile Front { get; set; }

        [Required]
        public IFormFile Back { get; set; }
    }
}
