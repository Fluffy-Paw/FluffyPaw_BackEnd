using CoreApiResponse;
using FluffyPaw_Application.DTO.Request.WalletRequest;
using FluffyPaw_Application.ServiceImplements;
using FluffyPaw_Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FluffyPaw_API.Controllers.Payment
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalletController : BaseController
    {
        private readonly IWalletService _paymentService;

        public WalletController(IWalletService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpGet("ViewWallet")]
        [Authorize]
        public async Task<IActionResult> ViewWallet()
        {
            var result = await _paymentService.ViewWallet();
            return CustomResult("Thông tin ví: ", result);
        }

        [HttpGet("ViewBalance")]
        [Authorize]
        public async Task<IActionResult> ViewBalance()
        {
            var result = await _paymentService.ViewBalance();
            return CustomResult("Số dư: ", result);
        }

        [HttpPatch("UpdateBankInfomation")]
        [Authorize]
        public async Task<IActionResult> UpdateBankInfomation(BankAccountRequest bankAccountRequest)
        {
            var result = await _paymentService.UpdateBankInfo(bankAccountRequest);
            return CustomResult("Cập nhật thông tin ngân hàng thành công.", result);
        }

        [HttpPatch("WithdrawMoney")]
        [Authorize]
        public async Task<IActionResult> WithdrawMoney(double amount)
        {
            var result = await _paymentService.WithdrawMoney(amount);
            return CustomResult("Rút tiền thành công, số dư mới: ", result);
        }

        [HttpPatch("DepositMoney")]
        [Authorize]
        public async Task<IActionResult> DepositMoney(double amount)
        {
            var result = await _paymentService.DepositMoney(amount);
            return CustomResult("Nạp tiền thành công, số dư mới: ", result);
        }
    }
}