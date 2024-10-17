using FluffyPaw_Application.DTO.Response.FilesResponse;
using FluffyPaw_Application.Mapper;
using FluffyPaw_Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Application.DTO.Response.StaffResponse
{
    public class TrackingResponse : IMapFrom<Tracking>, IMapFrom<Files>
    {
        public long Id { get; set; }

        public long BookingId { get; set; }

        public DateTimeOffset UploadDate { get; set; }

        public string? Description { get; set; }

        public bool Status { get; set; }

        public List<FileResponse> Files { get; set; }
    }
}
