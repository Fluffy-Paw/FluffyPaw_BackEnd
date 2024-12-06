using FluffyPaw_Application.DTO.Response.BookingResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Application.DTO.Request.EmailRequest
{
    public class SendReceiptRequest
    {
        public string Email { get; set; }
        public string CustomerName { get; set; }
        public List<BookingResponse> bookingResponses { get; set; }
    }
}
