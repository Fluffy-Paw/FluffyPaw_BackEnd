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
    public class Notification
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public long ReceiverId { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }

        public string Description { get; set; }

        public DateTimeOffset CreateDate { get; set; }

        public bool IsSeen { get; set; }

        public string Status { get; set; }

        [ForeignKey("ReceiverId")]
        public virtual Account Account { get; set; }

        public Notification()
        {
            CreateDate = CoreHelper.SystemTimeNow;
        }
    }
}
