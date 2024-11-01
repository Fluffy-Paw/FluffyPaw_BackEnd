using FluffyPaw_Application.Mapper;
using FluffyPaw_Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Application.DTO.Response.ConversationResponse
{
    public class ConversationResponse : IMapFrom<Conversation>
    {
        public long Id { get; set; }

        public long PoAccountId { get; set; }

        public string PoName { get; set; }

        public string PoAvatar { get; set; }

        public long StaffAccountId { get; set; }

        public string StoreName { get; set; }

        public string StoreAvatar { get; set; }

        public string LastMessege { get; set; }

        public bool IsOpen { get; set; }
    }
}
