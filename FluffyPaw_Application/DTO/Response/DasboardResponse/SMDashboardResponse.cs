using FluffyPaw_Domain.Entities;
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

        public int NumOfStores { get; set; }

        public int NumOfReports { get; set; }

        public List<double> Revenues { get; set; }

        public List<StoreServiceResponse> TopServices { get; set; }

    }
}
