using FluffyPaw_Application.Mapper;
using FluffyPaw_Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Application.DTO.Response.StoreManagerResponse
{
    public class StBookingResponse : IMapFrom<Booking>
    {
        public long Id { get; set; }

        public string Code { get; set; }

        public string PaymentMethod { get; set; }

        public double Cost { get; set; }

        public string? Description { get; set; }

        public DateTimeOffset CreateDate { get; set; }

        public DateTimeOffset StartTime { get; set; }

        public DateTimeOffset EndTime { get; set; }

        public string Status { get; set; }
    }
}
