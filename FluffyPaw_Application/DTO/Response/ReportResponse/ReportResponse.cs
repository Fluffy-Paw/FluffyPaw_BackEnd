using AutoMapper;
using FluffyPaw_Application.Mapper;
using FluffyPaw_Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Application.DTO.Response.ReportResponse
{
    public class ReportResponse : IMapFrom<Report>
    {
        public long Id { get; set; }

        public long SenderId { get; set; }

        public string SenderName { get; set; }

        public long TargetId { get; set; }

        public string TargetName { get; set; }

        public string ReportCateogyType { get; set; }

        public string ReportCategoryName { get; set; }

        public DateTimeOffset CreateDate { get; set; }

        public string Description { get; set; }

        public bool Status { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Report, ReportResponse>()
                .ForMember(dest => dest.ReportCategoryName, opt => opt.MapFrom(src => src.ReportCategory.Name))
                .ForMember(dest => dest.ReportCateogyType, opt => opt.MapFrom(src => src.ReportCategory.Type));
        }
    }
}
