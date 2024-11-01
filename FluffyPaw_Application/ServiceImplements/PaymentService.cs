using FluffyPaw_Application.DTO.Request.PaymentRequest;
using FluffyPaw_Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Net.payOS.Types;
using Net.payOS;
using Microsoft.AspNetCore.Http;
using FluffyPaw_Domain.CustomException;
using FluffyPaw_Domain.Interfaces;

namespace FluffyPaw_Application.ServiceImplements
{
    public class PaymentService : IPaymentService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly PayOS _payOS;
        private readonly IAuthentication _authentication;

        public PaymentService(IHttpContextAccessor httpContextAccessor, PayOS payOS, IAuthentication authentication)
        {
            _httpContextAccessor = httpContextAccessor;
            _payOS = payOS;
            _authentication = authentication;
        }

        public async Task<string> CreatePayment(CreatePaymentRequest createPaymentRequest)
        {
            if (createPaymentRequest.Amount < 2000) throw new CustomException.InvalidDataException("Bạn phải nạp tối thiểu 50000 VND.");
            int orderCode = int.Parse(DateTimeOffset.Now.ToString("ffffff"));
            ItemData item = new ItemData($"Nạp {createPaymentRequest.Amount}VND vào ví FluffyPay", 1, (int)createPaymentRequest.Amount);
            List<ItemData> items = new List<ItemData> { item };

            // Get the current request's base URL
            var request = _httpContextAccessor.HttpContext.Request;
            var baseUrl = $"{request.Scheme}://{request.Host}";

            PaymentData paymentData = new PaymentData(
                orderCode,
                createPaymentRequest.Amount,
                "Nap tien vao Fluffy Paw",
                items,
                $"{baseUrl}/wallet",
                $"{baseUrl}/wallet?amount={createPaymentRequest.Amount}"
            );

            CreatePaymentResult createPayment = await _payOS.createPaymentLink(paymentData);

            return createPayment.checkoutUrl;
        }
    }
}
