using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Domain.Entities
{
    public class StaffAddressFile
    {
        public long Id { get; set; }

        public long FileId { get; set; }

        public long StaffAddressId { get; set; }

        [ForeignKey("FileId")]
        public virtual Files Files { get; set; }

        [ForeignKey("StaffAddressId")]
        public virtual StaffAddress StaffAddress { get; set; }
    }
}
