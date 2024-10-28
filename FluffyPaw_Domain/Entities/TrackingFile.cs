using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Domain.Entities
{
    public class TrackingFile
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public long TrackingId { get; set; }

        public long FileId { get; set; }

        [ForeignKey("TrackingId")]
        public virtual Tracking Tracking { get; set; }

        [ForeignKey("FileId")]
        public virtual Files Files { get; set; }
    }
}
