using FluffyPaw_Application.Mapper;
using FluffyPaw_Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Application.DTO.Request.ConversationRequest
{
    public class ConversationRequest : IMapFrom<Conversation>
    {
        public long PersonId { get; set; }
    }
}
