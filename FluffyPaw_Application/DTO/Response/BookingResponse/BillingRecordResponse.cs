using FluffyPaw_Application.Mapper;
using FluffyPaw_Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Application.DTO.Response.BookingResponse
{
    public class BillingRecordResponse : IMapFrom<BillingRecord>, IMapFrom<Booking>
    {
        public long Id { get; set; }

        public long WalletId { get; set; }

        public long BookingId { get; set; }

        public double Amount { get; set; }

        public string Description { get; set; }

        public DateTimeOffset CreateDate { get; set; }
    }
}
