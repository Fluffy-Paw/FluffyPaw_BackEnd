using FluffyPaw_Application.Mapper;
using FluffyPaw_Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Application.DTO.Request.StoreServiceRequest
{
    public class CreateStoreServiceRequest : IMapFrom<StoreService>
    {
        public long ServiceId { get; set; }

        public List<CreateScheduleRequest> CreateScheduleRequests { get; set; }
    }
}
