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
    public class RegisterAccountPORequest : IMapFrom<Account>, IMapFrom<PetOwner>
    {
        [RegularExpression("^(0?)(3[2-9]|5[6|8|9]|7[0|6-9]|8[0-6|8|9]|9[0-4|6-9])[0-9]{7}$", ErrorMessage = "Số điện thoại này không tồn tại!")]
        [Required(ErrorMessage = "Số điện thoại là trường bắt buộc phải nhập.")]
        public string Phone { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập Username.")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập Password.")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập confirm password.")]
        public string ComfirmPassword { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập email.")]
        [EmailAddress(ErrorMessage = "Vui lòng nhập đúng định dạng email.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập tên.")]
        public string FullName { get; set; }
        public string Address { get; set; }
        public DateTimeOffset Dob { get; set; }
        public string Gender { get; set; }
    }
}
