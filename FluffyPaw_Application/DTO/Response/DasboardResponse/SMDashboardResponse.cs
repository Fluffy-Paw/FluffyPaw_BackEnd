using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Application.DTO.Response.DasboardResponse
{
    public class SMDashboardResponse
    {
        public int NumOfAll {  get; set; }

        public int NumOfPending { get; set; }

        public int NumOfAccepted { get; set; }

        public int NumOfCanceled { get; set; }

    }
}
