using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Application.DTO.Request.StoreManagerRequest
{
    public class RevenueRequest
    {
        public DateTime? startDate { get; set; }

        public DateTime? endDate { get; set; }

        public int? month { get; set; }

        public int? year { get; set; }
    }
}
