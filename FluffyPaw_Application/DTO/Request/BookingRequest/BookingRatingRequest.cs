using FluffyPaw_Application.Mapper;
using FluffyPaw_Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Application.DTO.Request.BookingRequest
{
    public class BookingRatingRequest : IMapFrom<BookingRating>
    {
        public int Vote { get; set; }

        public string? Description { get; set; }
    }
}
