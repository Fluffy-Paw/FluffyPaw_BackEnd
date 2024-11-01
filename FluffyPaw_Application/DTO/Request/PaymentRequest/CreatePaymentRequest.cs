using FluffyPaw_Application.Mapper;
using FluffyPaw_Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Application.DTO.Request.PaymentRequest
{
    public class CreatePaymentRequest
    {
        [Required(ErrorMessage = "Vui lòng nhập số tiền.")]
        public int Amount { get; set; }
    }
}
