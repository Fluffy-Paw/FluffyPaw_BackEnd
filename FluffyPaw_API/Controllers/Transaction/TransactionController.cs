using CoreApiResponse;
using FluffyPaw_Application.DTO.Request.TransactionRequest;
using FluffyPaw_Application.ServiceImplements;
using FluffyPaw_Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FluffyPaw_API.Controllers.Transaction
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : BaseController
    {
        private readonly ITransactionService _transactionService;

        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpPost("AddTransactions")]
        [Authorize]
        public async Task<IActionResult> AddTransactions([FromBody]TransactionRequest transactionRequest)
        {
            var result = await _transactionService.AddTransactions(transactionRequest);
            return CustomResult("Thêm giao dịch thành công.", result);
        }

        [HttpGet("GetTransactions")]
        [Authorize]
        public async Task<IActionResult> GetTransactions()
        {
            var result = await _transactionService.GetTransactions();
            return CustomResult("Danh sách giao dịch:", result);
        }

    }
}
