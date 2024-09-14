using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Domain.Entities
{
    public class PetFile
    {
        public long Id { get; set; }

        public long PetId { get; set; }

        public long FileId { get; set; }

        [ForeignKey("PetId")]
        public virtual Pet Pet { get; set; }

        [ForeignKey("FileId")]
        public virtual Files Files { get; set; }
    }
}
