using FluffyPaw_Application.Mapper;
using FluffyPaw_Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Application.DTO.Request.NotificationRequest
{
    public class NotificationRequest : IMapFrom<Notification>, IMapFrom<Account>
    {
        public long ReceiverId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

    }
}
