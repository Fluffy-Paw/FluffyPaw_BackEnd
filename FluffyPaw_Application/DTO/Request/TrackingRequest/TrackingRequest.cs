using FluffyPaw_Application.Mapper;
using FluffyPaw_Domain.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Application.DTO.Request.TrackingRequest
{
    public class TrackingRequest : IMapFrom<Tracking>, IMapFrom<TrackingFile>, IMapFrom<Files>
    {
        public long BookingId { get; set; }

        public string? Description { get; set; }

        public List<IFormFile> Files { get; set; }
    }
}
