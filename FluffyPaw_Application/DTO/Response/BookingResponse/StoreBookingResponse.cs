﻿using AutoMapper;
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
        public string FullName { get; set; }

        public string Phone { get; set; }

        public string ServiceName { get; set; }

        public DateTimeOffset CreateDate { get; set; }

        public DateTimeOffset StartTime { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Booking, StoreBookingResponse>()
                   .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.Pet.PetOwner.FullName))
                   .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.Pet.PetOwner.Phone))
                   .ForMember(dest => dest.ServiceName, opt => opt.MapFrom(src => src.StoreService.Service.Name))
                   .ForMember(dest => dest.CreateDate, opt => opt.MapFrom(src => src.CreateDate))
                   .ForMember(dest => dest.StartTime, opt => opt.MapFrom(src => src.StoreService.StartTime));
        }
    }
}