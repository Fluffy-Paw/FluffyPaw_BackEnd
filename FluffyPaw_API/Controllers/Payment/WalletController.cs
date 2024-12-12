using CoreApiResponse;
using FluffyPaw_Application.DTO.Request.NotificationRequest;
using FluffyPaw_Application.DTO.Request.TransactionRequest;
using FluffyPaw_Application.DTO.Request.WalletRequest;
using FluffyPaw_Application.ServiceImplements;
using FluffyPaw_Application.Services;
using FluffyPaw_Domain.Entities;
using FluffyPaw_Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FluffyPaw_API.Controllers.Payment
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalletController : BaseController
    {
        private readonly IWalletService _walletService;
        private readonly ITransactionService _transactionService;
        private readonly INotificationService _notificationService;

        public WalletController(IWalletService paymentService, ITransactionService transactionService, INotificationService notificationService)
        {
            _walletService = paymentService;
            _transactionService = transactionService;
            _notificationService = notificationService;
        }

        [HttpGet("ViewWallet")]
        [Authorize]
        public async Task<IActionResult> ViewWallet()
        {
            var result = await _walletService.ViewWallet();
            return CustomResult("Thông tin ví: ", result);
        }

        [HttpGet("ViewBalance")]
        [Authorize]
        public async Task<IActionResult> ViewBalance()
        {
            var result = await _walletService.ViewBalance();
            return CustomResult("Số dư: ", result);
        }

        [HttpPatch("UpdateBankInfomation")]
        [Authorize]
        public async Task<IActionResult> UpdateBankInfomation([FromForm]BankAccountRequest bankAccountRequest)
        {
            var result = await _walletService.UpdateBankInfo(bankAccountRequest);
            return CustomResult("Cập nhật thông tin ngân hàng thành công.", result);
        }

        [HttpPatch("WithdrawMoney/{amount}")]
        [Authorize]
        public async Task<IActionResult> WithdrawMoney(double amount)
        {
            var wallet = await _walletService.ViewWallet();
            var result = await _walletService.WithdrawMoney(amount);

            await _transactionService.AddTransactions(new TransactionRequest
            {
                OrderCode = int.Parse(DateTimeOffset.Now.ToString("ffffff")),
                Amount = amount,
                Type = "Rút tiền",
                WalletId = wallet.Id,
                BankName = wallet.BankName,
                BankNumber = wallet.Number
            });

            await _notificationService.CreateNotification(new NotificationRequest
            {
                Name = wallet.Id.ToString(),
                Type = NotificationType.WithDrawRequest.ToString(),
                ReceiverId = 1,
                Description = $"{wallet.Account.Username}/{amount}",
                ReferenceId = wallet.Id
            });

            return CustomResult("Rút tiền thành công, tiền sẽ chuyển vào ngân hàng của bạn trong vòng 1 ngày. Số dư mới: ", result);
        }

        [HttpPatch("DepositMoney/{amount}")]
        [Authorize]
        public async Task<IActionResult> DepositMoney(double amount)
        {
            var result = await _walletService.DepositMoney(amount);
            return CustomResult("Nạp tiền thành công, số dư mới: ", result);
        }
    }
}