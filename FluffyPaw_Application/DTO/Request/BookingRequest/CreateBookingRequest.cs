using FluffyPaw_Application.Mapper;
using FluffyPaw_Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Application.DTO.Request.BookingRequest
{
    public class CreateBookingRequest : IMapFrom<Booking>
    {
        public long PetId { get; set; }

        public long StoreServiceId { get; set; }

        public string PaymentMethod { get; set; }

        public string? Description { get; set; }
    }
}
