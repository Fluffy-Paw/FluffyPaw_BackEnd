using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Application.DTO.Request.BookingRequest
{
    public class CheckinRequest
    {
        public long Id { get; set; }

        public IFormFile CheckinImagge { get; set; }
    }
}
