using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Application.DTO.Response.DasboardResponse
{
    public class AdminDashboardResponse
    {
        public int TotalUser { get; set; }

        public int TotalSMs {  get; set; }

        public int TotalStore { get; set; }

        public int TotalPOs { get; set; }

        public List<double> WithdrawRevenues { get; set; }

        public List<double> DepositRevenues { get; set; }

    }
}
