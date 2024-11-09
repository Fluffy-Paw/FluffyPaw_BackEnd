using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Application.DTO.Response.PaymentResponse
{
    public class CreateDepositResponse
    {
        public string checkoutUrl {  get; set; }

        public long orderCode { get; set; }
    }
}
