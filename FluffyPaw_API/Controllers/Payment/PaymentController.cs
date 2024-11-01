using CoreApiResponse;
using FluffyPaw_Application.DTO.Request.PaymentRequest;
using FluffyPaw_Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FluffyPaw_API.Controllers.Payment
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : BaseController
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost("CreateDepositLink")]
        [Authorize]
        public async Task<IActionResult> CreateDepositLink([FromBody] CreatePaymentRequest createPaymentRequest)
        {
            var result = await _paymentService.CreatePayment(createPaymentRequest);
            return CustomResult(result);
        }
    }
}
