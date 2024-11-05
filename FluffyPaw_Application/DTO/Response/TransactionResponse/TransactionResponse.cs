using FluffyPaw_Application.Mapper;
using FluffyPaw_Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Application.DTO.Response.TransactionResponse
{
    public class TransactionResponse : IMapFrom<Transaction>
    {
        public string Type { get; set; }

        public double Ammount { get; set; }

        public long OrderCode { get; set; }

        public DateTimeOffset CreateTime { get; set; }
    }
}
