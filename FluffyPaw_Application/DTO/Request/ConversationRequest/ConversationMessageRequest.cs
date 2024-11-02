using FluffyPaw_Application.Mapper;
using FluffyPaw_Domain.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Application.DTO.Request.ConversationRequest
{
    public class ConversationMessageRequest : IMapFrom<ConversationMessage>, IMapFrom<MessageFile>, IMapFrom<Files>
    {
        public long ConversationId { get; set; }

        public string Content { get; set; }

        public long? ReplyMessageId { get; set; } = null;

        public List<IFormFile> Files { get; set; }
    }
}
