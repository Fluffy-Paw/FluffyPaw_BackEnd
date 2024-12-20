using AutoMapper;
using FluffyPaw_Application.Mapper;
using FluffyPaw_Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Application.DTO.Response.BookingResponse
{
    public class StoreBookingResponse : IMapFrom<Booking>, IMapFrom<PetOwner>, IMapFrom<Service>
    {
        public long Id { get; set; }

        public string Code { get; set; }

        public long PetId { get; set; } 

        public long PetOwnerAccountId { get; set; }

        public string FullName { get; set; }

        public string Phone { get; set; }

        public string ServiceName { get; set; }

        public string PaymentMethod { get; set; }

        public double Cost { get; set; }

        public DateTimeOffset CreateDate { get; set; }

        public DateTimeOffset StartTime { get; set; }

        public DateTimeOffset EndTime { get; set; } 

        public bool Checkin { get; set; }

        public bool CheckOut { get; set; }

        public string Status { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Booking, StoreBookingResponse>()
                   .ForMember(dest => dest.PetId, opt => opt.MapFrom(src => src.PetId))
                   .ForMember(dest => dest.PetOwnerAccountId, opt => opt.MapFrom(src => src.Pet.PetOwner.AccountId))
                   .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.Pet.PetOwner.FullName))
                   .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.Pet.PetOwner.Phone))
                   .ForMember(dest => dest.ServiceName, opt => opt.MapFrom(src => src.StoreService.Service.Name))
                   .ForMember(dest => dest.Cost, opt => opt.MapFrom(src => src.StoreService.Service.Cost))
                   .ForMember(dest => dest.CreateDate, opt => opt.MapFrom(src => src.CreateDate))
                   .ForMember(dest => dest.StartTime, opt => opt.MapFrom(src => src.StoreService.StartTime));
        }
    }
}
