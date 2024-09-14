using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Domain.Entities
{
    public class CertificateFile
    {
        public long Id { get; set; }

        public long CertificateId { get; set; }

        public long FileId { get; set; }

        [ForeignKey("CertificateId")]
        public virtual Certificate Certificate { get; set; }

        [ForeignKey("FileId")]
        public virtual Files Files { get; set; }
    }
}
