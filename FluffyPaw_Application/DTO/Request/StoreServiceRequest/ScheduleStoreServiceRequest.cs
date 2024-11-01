using FluffyPaw_Application.Mapper;
using FluffyPaw_Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Application.DTO.Request.StoreServiceRequest
{
    public class ScheduleStoreServiceRequest : IMapFrom<StoreService>
    {
        public List<long> Id { get; set; }

        public int DuplicateNumber { get; set; }
    }
}
