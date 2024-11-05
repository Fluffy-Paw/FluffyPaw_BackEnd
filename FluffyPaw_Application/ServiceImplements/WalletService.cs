using AutoMapper;
using FluffyPaw_Application.DTO.Request.NotificationRequest;
using FluffyPaw_Application.DTO.Request.WalletRequest;
using FluffyPaw_Application.Services;
using FluffyPaw_Domain.CustomException;
using FluffyPaw_Domain.Entities;
using FluffyPaw_Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Application.ServiceImplements
{
    public class WalletService : IWalletService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAuthentication _authentication;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IFirebaseConfiguration _firebaseConfiguration;
        private readonly INotificationService _notificationService;

        public WalletService(IUnitOfWork unitOfWork, IMapper mapper, IAuthentication authentication, IHttpContextAccessor httpContextAccessor, IFirebaseConfiguration firebaseConfiguration, INotificationService notificationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _authentication = authentication;
            _httpContextAccessor = httpContextAccessor;
            _firebaseConfiguration = firebaseConfiguration;
            _notificationService = notificationService;
        }

        public async Task<double> DepositMoney(double amount)
        {
            var userId = _authentication.GetUserIdFromHttpContext(_httpContextAccessor.HttpContext);
            if (userId == 0) throw new CustomException.UnAuthorizedException("Vui lòng đăng nhập để xem ví.");

            if (amount < 0) throw new CustomException.InvalidDataException("Bạn không thể nhập số âm.");

            var wallet = _unitOfWork.WalletRepository.Get(w => w.AccountId == userId).FirstOrDefault();
            if (wallet == null)
            {
                throw new CustomException.DataNotFoundException("Không tìm thấy ví.");
            }

            wallet.Balance += amount;
            await _unitOfWork.SaveAsync();

            return wallet.Balance;
        }

        public async Task<bool> UpdateBankInfo(BankAccountRequest bankAccountRequest)
        {
            var userId = _authentication.GetUserIdFromHttpContext(_httpContextAccessor.HttpContext);
            if (userId == 0) throw new CustomException.UnAuthorizedException("Vui lòng đăng nhập để xem ví.");

            var wallet = _unitOfWork.WalletRepository.Get(w => w.AccountId == userId).FirstOrDefault();
            if (wallet == null)
            {
                throw new CustomException.DataNotFoundException("Không tìm thấy ví.");
            }

            _mapper.Map(bankAccountRequest,wallet);
            if (bankAccountRequest.ImageQR != null) wallet.QR = await _firebaseConfiguration.UploadImage(bankAccountRequest.ImageQR);
            await _unitOfWork.SaveAsync();

            return true;
        }

        public async Task<double> ViewBalance()
        {
            var userId = _authentication.GetUserIdFromHttpContext(_httpContextAccessor.HttpContext);
            if (userId == 0) throw new CustomException.UnAuthorizedException("Vui lòng đăng nhập để xem ví.");

            var wallet = _unitOfWork.WalletRepository.Get(w => w.AccountId == userId).FirstOrDefault();
            if (wallet == null)
            {
                throw new CustomException.DataNotFoundException("Không tìm thấy ví.");
            }

            return wallet.Balance;
        }

        public async Task<Wallet> ViewWallet()
        {
            var userId = _authentication.GetUserIdFromHttpContext(_httpContextAccessor.HttpContext);
            if (userId == 0) throw new CustomException.UnAuthorizedException("Vui lòng đăng nhập để xem ví.");

            var wallet = _unitOfWork.WalletRepository.Get(w => w.AccountId == userId).FirstOrDefault();
            if (wallet == null)
            {
                throw new CustomException.DataNotFoundException("Không tìm thấy ví.");
            }

            return wallet;
        }

        public async Task<double> WithdrawMoney(double amount)
        {
            var userId = _authentication.GetUserIdFromHttpContext(_httpContextAccessor.HttpContext);
            if (userId == 0) throw new CustomException.UnAuthorizedException("Vui lòng đăng nhập để xem ví.");

            if (amount < 0) throw new CustomException.InvalidDataException("Bạn không thể nhập số âm.");

            var wallet = _unitOfWork.WalletRepository.Get(w => w.AccountId == userId, includeProperties: "Account").FirstOrDefault();
            if (wallet == null) throw new CustomException.DataNotFoundException("Không tìm thấy ví.");

            if (wallet.BankName == null || wallet.Number == null) throw new CustomException.InvalidDataException("Vui lòng nhập tài khoản ngân hàng trước khi rút tiền.");

            if (wallet.Balance < amount) throw new CustomException.InvalidDataException("Số dư của bạn không đủ.");
            wallet.Balance -= amount;
            await _unitOfWork.SaveAsync();

            await _notificationService.CreateNotification(new NotificationRequest
            {
                Name = "Rút tiền",
                Type = "Deposit Request",
                ReceiverId = 1,
                Description = $"Tài khoản {wallet.Account.Username} muốn rút {amount} về số tài khoản {wallet.Number} của ngân hàng {wallet.BankName}."
            });

            return wallet.Balance;
        }
    }
}
