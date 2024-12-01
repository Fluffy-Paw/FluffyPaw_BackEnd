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
    public class BookingRatingResponse : IMapFrom<BookingRating>, IMapFrom<PetOwner>, IMapFrom<Account>
    {
        public long Id { get; set; }

        public long BookingId { get; set; }

        public long PetOwnerId { get; set; }

        public string FullName { get; set; }

        public string Avatar { get; set; }

        public int ServiceVote { get; set; }

        public int StoreVote { get; set; }

        public string Description { get; set; }

        public string Image { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<BookingRating, BookingRatingResponse>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.PetOwner.FullName))
                .ForMember(dest => dest.Avatar, opt => opt.MapFrom(src => src.PetOwner.Account.Avatar));
        }
    }
}
