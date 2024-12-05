using FluffyPaw_Domain.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Domain.Entities
{
    public class ConversationMessage
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public long ConversationId { get; set; }

        public long SenderId { get; set; }

        public DateTimeOffset CreateTime { get; set; }

        public string? Content { get; set; }

        public bool IsSeen { get; set; }

        public long? ReplyMessageId { get; set; }

        [ForeignKey("ConversationId")]
        public virtual Conversation Conversation { get; set; }

        [ForeignKey("ReplyMessageId")]
        public virtual ConversationMessage ReplyMessage { get; set; }

        public virtual ICollection<ConversationMessage> Replies { get; set; }

        public virtual ICollection<MessageFile> MessageFiles { get; set; }

        public ConversationMessage()
        {
            CreateTime = CoreHelper.SystemTimeNow;
        }
    }
}
