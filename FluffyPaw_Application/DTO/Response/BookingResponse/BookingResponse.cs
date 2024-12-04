using AutoMapper;
using FluffyPaw_Application.DTO.Response.ServiceResponse;
using FluffyPaw_Application.Mapper;
using FluffyPaw_Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Application.DTO.Response.BookingResponse
{
    public class BookingResponse : IMapFrom<Booking>, IMapFrom<StoreService>, IMapFrom<Store>, IMapFrom<Service>
    {
        public long Id { get; set; }

        public string Code { get; set; }

        public long PetId { get; set; }

        public string PetName { get; set; }

        public long ServiceId { get; set; }

        public string ServiceName { get; set; }

        public long StoreId { get; set; }

        public string StoreName { get; set; }

        public string Address { get; set; }

        public long StoreServiceId { get; set; }

        public string PaymentMethod { get; set; }

        public double Cost { get; set; }

        public string? Description { get; set; }

        public DateTimeOffset CreateDate { get; set; }

        public DateTimeOffset StartTime { get; set; }

        public DateTimeOffset EndTime { get; set; }

        public bool Checkin { get; set; }

        public DateTimeOffset? CheckinTime { get; set; }

        public string CheckinImage { get; set; }

        public bool CheckOut { get; set; }

        public DateTimeOffset? CheckOutTime { get; set; }

        public string CheckoutImage { get; set; }

        public string Status { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Booking, BookingResponse>()
                .ForMember(dest => dest.ServiceId, opt => opt.MapFrom(src => src.StoreService.ServiceId))
                .ForMember(dest => dest.ServiceName, opt => opt.MapFrom(src => src.StoreService.Service.Name))
                .ForMember(dest => dest.StoreId, opt => opt.MapFrom(src => src.StoreService.StoreId))
                .ForMember(dest => dest.StoreName, opt => opt.MapFrom(src => src.StoreService.Store.Name))
                .ForMember(dest => dest.PetName, opt => opt.MapFrom(src => src.Pet.Name))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.StoreService.Store.Address));
        }
    }
}
