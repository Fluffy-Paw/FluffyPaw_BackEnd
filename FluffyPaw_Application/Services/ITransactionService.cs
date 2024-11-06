using FluffyPaw_Application.DTO.Request.TransactionRequest;
using FluffyPaw_Application.DTO.Response.TransactionResponse;
using FluffyPaw_Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Application.Services
{
    public interface ITransactionService
    {
        Task<IEnumerable<TransactionResponse>> GetTransactions();
        Task<bool> AddTransactions(TransactionRequest transaction);
        Task<bool> CheckReceipt(long orderCode);
    }
}
