using FluffyPaw_Application.DTO.Request.WalletRequest;
using FluffyPaw_Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Application.Services
{
    public interface IPaymentService
    {
        Task<double> WithdrawMoney(double amount);
        Task<double> DepositMoney(double amount);
        Task<double> ViewBalance();
        Task<Wallet> ViewWallet();
        Task<bool> UpdateBankInfo(BankAccountRequest bankAccountRequest);
    }
}
