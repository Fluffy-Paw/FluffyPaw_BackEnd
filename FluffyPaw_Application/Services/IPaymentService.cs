using FluffyPaw_Application.DTO.Request.PaymentRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Application.Services
{
    public interface IPaymentService
    {
        Task<string> CreateDepositRequest(CreatePaymentRequest createPaymentRequest);
        Task<bool> CancelPayment(long orderCode);
        Task<bool> CheckDepositResult(long orderCode);
        Task<bool> PayBooking(string serviceName, double amount);
    }
}
