using FluffyPaw_Application.Mapper;
using FluffyPaw_Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Application.DTO.Response.StoreServiceResponse
{
    public class StoreServicePOResponse : IMapFrom<StoreService>
    {
        public long Id { get; set; }

        public string StoreName { get; set; }

        public long StoreId { get; set; }

        public string ServiceName { get; set; }

        public long ServiceId { get; set; }

        public string ServiceType { get; set; }

        public long ServiceTypeId { get; set; }

        public string BrandName { get; set; }

        public long BrandId { get; set; }

        public double Cost { get; set; }

        public string? Image {  get; set; }

        public DateTimeOffset StartTime { get; set; }

        public int LimitPetOwner { get; set; }

        public int CurrentPetOwner { get; set; }

        public string Status { get; set; }

        public TimeSpan Duration { get; set; }

        public string? Description { get; set; }

        public int BookingCount { get; set; }

        public float TotalRating { get; set; }


    }
}
