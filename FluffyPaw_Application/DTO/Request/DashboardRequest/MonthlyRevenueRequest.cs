using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Application.DTO.Request.DashboardRequest
{
    public class MonthlyRevenueRequest
    {
        public string ServiceName { get; set; }
        public int Month { get; set; }
    }
}
