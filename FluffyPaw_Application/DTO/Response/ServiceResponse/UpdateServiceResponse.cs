using FluffyPaw_Application.Mapper;
using FluffyPaw_Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Application.DTO.Response.ServiceResponse
{
    public class UpdateServiceResponse : IMapFrom<Service>
    {
        public long Id { get; set; }

        public long ServiceTypeId { get; set; }

        public long BrandId { get; set; }

        public string Name { get; set; }

        public string Image { get; set; }

        public TimeSpan Duration { get; set; }

        public double Cost { get; set; }

        public string Description { get; set; }

        public int BookingCount { get; set; }

        public float TotalRating { get; set; }

        public bool Status { get; set; }
    }
}
