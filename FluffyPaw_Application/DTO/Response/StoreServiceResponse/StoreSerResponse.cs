using FluffyPaw_Application.Mapper;
using FluffyPaw_Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Application.DTO.Response.StoreServiceResponse
{
    public class StoreSerResponse : IMapFrom<StoreService>
    {
        public long Id { get; set; }

        public long StoreId { get; set; }

        public long ServiceId { get; set; }

        public DateTimeOffset StartTime { get; set; }

        public int LimitPetOwner { get; set; }

        public int CurrentPetOwner { get; set; }

        public string Status { get; set; }
    }
}
