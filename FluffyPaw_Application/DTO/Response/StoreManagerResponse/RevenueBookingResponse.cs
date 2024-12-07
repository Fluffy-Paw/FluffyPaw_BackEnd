using FluffyPaw_Application.DTO.Response.BookingResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Application.DTO.Response.StoreManagerResponse
{
    public class RevenueBookingResponse
    {
        public long StoreId { get; set; }
        public double StoreRevenue { get; set; }
            
        public List<StBookingResponse> Bookings { get; set; }
    }
}
