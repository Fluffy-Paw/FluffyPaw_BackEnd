using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Domain.Entities
{
    public class ServiceFile
    {
        public long Id { get; set; }

        public long ServiceId { get; set; }

        public long FileId { get; set; }

        [ForeignKey("ServiceId")]
        public virtual Service Service { get; set; }

        [ForeignKey("FileId")]
        public virtual Files Files { get; set; }
    }
}
