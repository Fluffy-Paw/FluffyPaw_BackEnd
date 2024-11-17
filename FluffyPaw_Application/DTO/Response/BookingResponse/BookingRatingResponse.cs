using FluffyPaw_Application.Mapper;
using FluffyPaw_Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Application.DTO.Response.BookingResponse
{
    public class BookingRatingResponse : IMapFrom<BookingRating>
    {
        public long Id { get; set; }

        public long BookingId { get; set; }

        public long PetOwnerId { get; set; }

        public int Vote { get; set; }

        public string? Description { get; set; }
    }
}
