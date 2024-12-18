﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Domain.Entities
{
    public class StoreFile
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public long FileId { get; set; }

        public long StoreId { get; set; }

        [ForeignKey("FileId")]
        public virtual Files Files { get; set; }

        [ForeignKey("StoreId")]
        public virtual Store Store { get; set; }
    }
}
