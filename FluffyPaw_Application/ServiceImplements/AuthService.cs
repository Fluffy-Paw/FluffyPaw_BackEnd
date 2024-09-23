﻿using AutoMapper;
using FluffyPaw_Application.DTO.Request.AuthRequest;
using FluffyPaw_Application.DTO.Response.AuthResponse;
using FluffyPaw_Application.Services;
using FluffyPaw_Domain.CustomException;
using FluffyPaw_Domain.Entities;
using FluffyPaw_Domain.Interfaces;
using FluffyPaw_Domain.Utils;
using FluffyPaw_Repository.Enum;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Application.ServiceImplements
{
    public class AuthService : IAuthService
    {
        private readonly IHashing _hashing;
        private readonly IAuthentication _authentication;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public AuthService(IAuthentication authentication, IUnitOfWork unitOfWork, IMapper mapper, IConfiguration configuration, IHashing hashing)
        {
            _authentication = authentication;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _configuration = configuration;
            _hashing = hashing;
        }

        public async Task<bool> RegisterPO(RegisterAccountPORequest registerAccountPORequest)
        {
            if (registerAccountPORequest.ComfirmPassword != registerAccountPORequest.Password)
            {
                throw new CustomException.InvalidDataException("Password và ConfirmPassword không trùng khớp.");
            }

            var existingAccount = _unitOfWork.PetOwnerRepository.Get(po => po.Phone == registerAccountPORequest.Phone);
            if (existingAccount.Any())
            {
                throw new CustomException.DataExistException("Số điện thoại này đã tồn tại trong hệ thống");
            }

            var account = _mapper.Map<Account>(registerAccountPORequest);
            account.RoleName = RoleName.PetOwner.ToString();
            account.Avatar = "https://cdn-icons-png.flaticon.com/512/10892/10892514.png";
            account.Password = _hashing.SHA512Hash(registerAccountPORequest.Password);
            account.Status = true;
            _unitOfWork.AccountRepository.Insert(account);
            _unitOfWork.Save();

            var wallet = new Wallet
            {
                AccountId = account.Id,
                Balance = 0
            };
            _unitOfWork.WalletRepository.Insert(wallet);

            var po = _mapper.Map<PetOwner>(registerAccountPORequest);
            po.AccountId = account.Id;
            po.Status = "Active";
            _unitOfWork.PetOwnerRepository.Insert(po);

            return true;
        }

        public async Task<bool> RegisterSM(RegisterAccountSMRequest registerAccountSMRequest)
        {
            if (registerAccountSMRequest.ComfirmPassword != registerAccountSMRequest.Password)
            {
                throw new CustomException.InvalidDataException("Password và ConfirmPassword không trùng khớp.");
            }

            var account = _mapper.Map<Account>(registerAccountSMRequest);
            account.RoleName = RoleName.StoreManager.ToString();
            account.Avatar = "https://cdn-icons-png.flaticon.com/512/10892/10892514.png";
            account.Password = _hashing.SHA512Hash(registerAccountSMRequest.Password);
            account.Status = true;
            _unitOfWork.AccountRepository.Insert(account);
            _unitOfWork.Save();

            var wallet = new Wallet
            {
                AccountId = account.Id,
                Balance = 0
            };
            _unitOfWork.WalletRepository.Insert(wallet);

            var sm = _mapper.Map<StoreManager>(registerAccountSMRequest);
            sm.AccountId = account.Id;
            sm.Status = false;
            _unitOfWork.StoreManagerRepository.Insert(sm);

            return true;
        }

        public async Task<string> Login(LoginRequest loginRequest)
        {
            string hashedPass = _hashing.SHA512Hash(loginRequest.Password);
            IEnumerable<Account> check = _unitOfWork.AccountRepository.Get(x =>
                x.Username.Equals(loginRequest.Username)
                && x.Password.Equals(hashedPass)
            );
            if (!check.Any())
            {
                throw new CustomException.InvalidDataException(HttpStatusCode.BadRequest.ToString(), $"Tài khoản hoặc mật khẩu không đúng.");
            }

            Account account = check.First();
            if (account.Status == false)
            {
                throw new CustomException.InvalidDataException(HttpStatusCode.BadRequest.ToString(), $"Tài khoản chưa được kích hoạt.");
            }




            string token = _authentication.GenerateJWTToken(account);
            return token;
        }
    }
}