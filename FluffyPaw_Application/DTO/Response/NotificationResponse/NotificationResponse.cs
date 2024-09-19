using FluffyPaw_Application.Mapper;
using FluffyPaw_Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Application.DTO.Response.NotificationResponse
{
    public class NotificationResponse : IMapFrom<Notification>
    {
        public long Id { get; set; }

        public long ReceiverId { get; set; }


        public string Description { get; set; }

        public DateTimeOffset CreateDate { get; set; }

        public bool IsSeen { get; set; }

    }
}
