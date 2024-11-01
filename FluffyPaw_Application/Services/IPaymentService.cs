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
        Task<string> CreatePayment(CreatePaymentRequest createPaymentRequest);
    }
}
