using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Domain.Entities
{
    public class MessageFile
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public long MessageId { get; set; }
        
        public long FileId { get; set; }

        [ForeignKey("MessageId")]
        public virtual ConversationMessage ConversationMessage { get; set; }

        [ForeignKey("FileId")]
        public virtual Files Files { get; set; }
    }
}
