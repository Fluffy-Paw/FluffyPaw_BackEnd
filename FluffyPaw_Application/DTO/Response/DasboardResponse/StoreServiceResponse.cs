using FluffyPaw_Application.Mapper;
using FluffyPaw_Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Application.DTO.Response.DasboardResponse
{
    public class StoreServiceResponse
    {
        public int Id { get; set; }
        public string StoreName { get; set; }

        public string ServiceName { get; set; }

        public int NumberOfBooking { get; set; }
    }
}
