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
    public class Report
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public long SenderId { get; set; }

        public long TargetId { get; set; }

        public long ReportCategoryId { get; set; }

        public DateTimeOffset CreateDate { get; set; }

        public string? Description { get; set; }

        public bool Status { get; set; }

        [ForeignKey("SenderId")]
        public virtual Account SenderAccount { get; set; }
        
        [ForeignKey("TargetId")]
        public virtual Account TargetAccount { get; set; }

        [ForeignKey("ReportCategoryId")]
        public virtual ReportCategory ReportCategory { get; set; }

        public Report()
        {
            CreateDate = CoreHelper.SystemTimeNow;
        }
    }
}
