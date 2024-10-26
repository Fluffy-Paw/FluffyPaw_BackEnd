using FluffyPaw_Application.Mapper;
using FluffyPaw_Domain.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Application.DTO.Request.WalletRequest
{
    public class BankAccountRequest : IMapFrom<Wallet>
    {
        [Required(ErrorMessage = "Vui lòng nhập tên ngân hàng.")]
        public string BankName { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập số tài khoản.")]
        public string Number { get; set; }

        public IFormFile? ImageQR { get; set; }
    }
}
