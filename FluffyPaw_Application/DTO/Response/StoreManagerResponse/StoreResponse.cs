using AutoMapper;
using FluffyPaw_Application.DTO.Response.FilesResponse;
using FluffyPaw_Application.DTO.Response.ServiceResponse;
using FluffyPaw_Application.Mapper;
using FluffyPaw_Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Application.DTO.Response.StoreManagerResponse
{
    public class StoreResponse : IMapFrom<Store>, IMapFrom<Brand>, IMapFrom<Account>
    {
        public long Id { get; set; }

        public long AccountId { get; set; }

        public string Username { get; set; }

        public long BrandId { get; set; }

        public string BrandName { get; set; }

        public string Logo { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public string Phone { get; set; }

        public float TotalRating { get; set; }

        public bool Status { get; set; }

        public Account Account { get; set; }

        public List<FileResponse> Files { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Store, StoreResponse>()
                .ForMember(dest => dest.BrandName, opt => opt.MapFrom(src => src.Brand.Name))
                .ForMember(dest => dest.Logo, opt => opt.MapFrom(src => src.Brand.Logo))
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Account.Username));
        }
    }
}
