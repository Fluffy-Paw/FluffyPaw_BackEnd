using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Domain.Entities
{
    public class Conversation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public long PoAccountId { get; set; }

        public long StaffAccountId { get; set; }

        public string? LastMessege { get; set; }

        public bool IsOpen { get; set; }

        public long? CloseAccountId { get; set; }

        [ForeignKey("PoAccountId")]
        public virtual Account PoAccount { get; set; }

        [ForeignKey("StaffAccountId")]
        public virtual Account StaffAccount { get; set; }

        [ForeignKey("CloseAccountId")]
        public virtual Account CloseAccount { get; set; }

        public virtual ICollection<ConversationMessage> ConversationMessages { get; set; }
    }
}
