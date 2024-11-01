using FluffyPaw_Application.DTO.Response.FilesResponse;
using FluffyPaw_Application.Mapper;
using FluffyPaw_Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Application.DTO.Response.ConversationResponse
{
    public class ConversationMessageResponse : IMapFrom<ConversationMessage>
    {
        public long Id { get; set; }

        public long ConversationId { get; set; }

        public long SenderId { get; set; }

        public DateTimeOffset CreateTime { get; set; }

        public string Content { get; set; }

        public bool IsSeen { get; set; }

        public DateTimeOffset? DeleteAt { get; set; }

        public bool IsDelete { get; set; }

        public long ReplyMessageId { get; set; }

        public List<FileResponse> Files { get; set; }
    }
}
