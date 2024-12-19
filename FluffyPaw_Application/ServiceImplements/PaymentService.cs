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
using FluffyPaw_Domain.Entities;
using FluffyPaw_Application.DTO.Request.TransactionRequest;
using FluffyPaw_Application.DTO.Response.PaymentResponse;
using static System.Net.WebRequestMethods;

namespace FluffyPaw_Application.ServiceImplements
{
    public class PaymentService : IPaymentService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly PayOS _payOS;
        private readonly IAuthentication _authentication;
        private readonly IWalletService _walletService;
        private readonly ITransactionService _transactionService;

        public PaymentService(IHttpContextAccessor httpContextAccessor, PayOS payOS, IAuthentication authentication, IWalletService walletService, ITransactionService transactionService)
        {
            _httpContextAccessor = httpContextAccessor;
            _payOS = payOS;
            _authentication = authentication;
            _walletService = walletService;
            _transactionService = transactionService;
        }

        public async Task<bool> CancelPayment(long orderCode)
        {
            try
            {
                PaymentLinkInformation paymentLinkInformation = await _payOS.cancelPaymentLink(orderCode);
                return true;
            }
            catch (System.Exception exception)
            {

                Console.WriteLine(exception);
                return false;
            }
        }
        public async Task<bool> CheckDepositResult(long orderCode)
        {
            var wallet = await _walletService.ViewWallet();
            try
            {
                PaymentLinkInformation paymentLinkInformation = await _payOS.getPaymentLinkInformation(orderCode);
                if (paymentLinkInformation.status.Equals("PAID") && await _transactionService.CheckReceipt(orderCode) == false)
                {
                    var newTransaction = new TransactionRequest
                    {
                        OrderCode = orderCode,
                        Type = "Nạp tiền",
                        Amount = paymentLinkInformation.amount,
                        WalletId = wallet.Id
                    };
                    await _transactionService.AddTransactions(newTransaction);
                    await _walletService.DepositMoney(paymentLinkInformation.amount);
                    return true;
                }
                return false;
            }
            catch (System.Exception exception)
            {
                Console.WriteLine(exception);
                return false;
            }
        }

        public async Task<CreateDepositResponse> CreateDepositRequest(CreatePaymentRequest createPaymentRequest)
        {
            if (createPaymentRequest.Amount < 2000) throw new CustomException.InvalidDataException("Bạn phải nạp tối thiểu 2000 VND.");
            int orderCode = int.Parse(DateTimeOffset.Now.ToString("ffffff"));
            ItemData item = new ItemData($"Nạp {createPaymentRequest.Amount}VND vào ví FluffyPay", 1, (int)createPaymentRequest.Amount);
            List<ItemData> items = new List<ItemData> { item };

            // Get the current request's base URL
            var request = _httpContextAccessor.HttpContext.Request;
            //var baseUrl = $"{request.Scheme}://{request.Host}";
            var baseUrl = "https://fluffy-paw.vercel.app";

            PaymentData paymentData = new PaymentData(
                orderCode,
                createPaymentRequest.Amount,
                "Nap tien vao Fluffy Paw",
                items,
                $"{baseUrl}/wallet",
                $"{baseUrl}/wallet"
                //$"http://localhost:3000/wallet",
                //$"http://localhost:3000/wallet"
            );

            CreatePaymentResult createPayment = await _payOS.createPaymentLink(paymentData);


            return new CreateDepositResponse{ checkoutUrl = createPayment.checkoutUrl, orderCode = orderCode };
        }

        //public async Task<bool> PayBooking(string serviceName, double amount)
        //{
        //    var wallet = await _walletService.ViewWallet();

        //    int orderCode = int.Parse(DateTimeOffset.Now.ToString("ffffff"));
        //    var newTransaction = new TransactionRequest
        //    {
        //        OrderCode = orderCode,
        //        Type = $"Thanh toán dịch vụ {serviceName}",
        //        Amount = amount,
        //        WalletId = wallet.Id,
        //    };

        //    await _walletService.WithdrawMoney(amount);
        //    await _transactionService.AddTransactions(newTransaction);
            
        //    return true;
        //}
    }
}
