using FluffyPaw_Application.Mapper;
using FluffyPaw_Domain.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Application.DTO.Request.StoreManagerRequest
{
    public class StoreRequest : IMapFrom<Store>, IMapFrom<Account>
    {
        [Required(ErrorMessage = "Vui lòng nhập Username.")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập Password.")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập confirm password.")]
        public string ComfirmPassword { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập email.")]
        [EmailAddress(ErrorMessage = "Vui lòng nhập đúng định dạng email.")]
        public string Email { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public string Phone { get; set; }

        public List<IFormFile> File { get; set; }
    }
}
