using FluffyPaw_Application.Mapper;
using FluffyPaw_Domain.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Application.DTO.Request.VacineRequest
{
    public class VaccineRequest : IMapFrom<VaccineHistory>, IMapFrom<Pet>
    {
        public long PetId { get; set; }

        public IFormFile? Image { get; set; }

        public float PetCurrentWeight { get; set; }

        public DateTimeOffset VaccineDate { get; set; }

        public DateTimeOffset NextVaccineDate { get; set; }

        public string? Description { get; set; }
    }
}
