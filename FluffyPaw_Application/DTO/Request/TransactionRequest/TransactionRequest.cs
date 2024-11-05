using FluffyPaw_Application.Mapper;
using FluffyPaw_Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Application.DTO.Request.TransactionRequest
{
    public class TransactionRequest : IMapFrom<Transaction>
    {
        public long WalletId { get; set; }

        public string Type { get; set; }

        public double Amount { get; set; }

        public long OrderCode { get; set; }
    }
}
