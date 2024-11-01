using FluffyPaw_Domain.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Domain.Entities
{
    public class Files
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public string File { get; set; }

        public DateTimeOffset CreateDate { get; set; }

        public bool Status { get; set; }

        public Files()
        {
            CreateDate = CoreHelper.SystemTimeNow;
        }
    }
}
