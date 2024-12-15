using AutoMapper;
using FluffyPaw_Application.DTO.Response.FilesResponse;
using FluffyPaw_Application.DTO.Response.PetOwnerResponse;
using FluffyPaw_Application.Mapper;
using FluffyPaw_Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Application.DTO.Response.StoreServiceResponse
{
    public  class StStoreServiceResponse : IMapFrom<Store>, IMapFrom<Files>
    {
        public long Id { get; set; }

        public long BrandId { get; set; }

        public string BrandName { get; set; }

        public string Name { get; set; }

        public long AccountId { get; set; }

        public string OperatingLicense { get; set; }

        public string Address { get; set; }

        public string Phone { get; set; }

        public float TotalRating { get; set; }

        public bool Status { get; set; }

        public List<FileResponse> Files { get; set; }

        public List<StoreSerResponse> StoreServices { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Store, StStoreServiceResponse>()
                .ForMember(dest => dest.Files, opt => opt.MapFrom(src => src.Files))
                .ForMember(dest => dest.BrandName, opt => opt.MapFrom(src => src.Brand.Name))
                .ForMember(dest => dest.StoreServices, opt => opt.MapFrom(src => src.StoreServices));
        }
    }
}
