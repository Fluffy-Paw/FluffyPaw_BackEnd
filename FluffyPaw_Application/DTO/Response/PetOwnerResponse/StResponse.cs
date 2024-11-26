using FluffyPaw_Application.Mapper;
using FluffyPaw_Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Application.DTO.Response.PetOwnerResponse
{
    public class StResponse : IMapFrom<Store>
    {
        public long Id { get; set; }

        public long BrandId { get; set; }

        public string Name { get; set; }

        public string OperatingLicense { get; set; }

        public string Address { get; set; }

        public string Phone { get; set; }

        public float TotalRating { get; set; }

        public bool Status { get; set; }

        public List<Files> Files { get; set; }
    }
}
