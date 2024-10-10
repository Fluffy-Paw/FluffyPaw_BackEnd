using FluffyPaw_Application.Mapper;
using FluffyPaw_Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Application.DTO.Request.StoreServiceRequest
{
    public class UpdateStoreServiceRequest : IMapFrom<StoreService>
    {
        public DateTimeOffset StartTime { get; set; }

        public int LimitPetOwner { get; set; }
    }
}
