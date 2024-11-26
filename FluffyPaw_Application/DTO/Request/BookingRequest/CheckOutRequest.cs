using FluffyPaw_Application.Mapper;
using FluffyPaw_Domain.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Application.DTO.Request.BookingRequest
{
    public class CheckOutRequest : IMapFrom<VaccineHistory>
    {
        public long Id { get; set; }

        public IFormFile? Image { get; set; }

        public string? Name { get; set; }

        public float? PetCurrentWeight { get; set; }

        public DateTimeOffset? NextVaccineDate { get; set; }

        public string? Description { get; set; }
    }
}
