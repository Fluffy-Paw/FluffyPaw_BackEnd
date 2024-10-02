using FluffyPaw_Application.Mapper;
using FluffyPaw_Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Application.DTO.Response.VaccineResponse
{
    public class ListVaccineResponse : IMapFrom<VaccineHistory>
    {
        public long Id { get; set; }

        public string? Image { get; set; }

        public DateTimeOffset VaccineDate { get; set; }

        public string? Description { get; set; }
    }
}
