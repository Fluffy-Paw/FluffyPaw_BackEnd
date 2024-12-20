using AutoMapper;
using FluffyPaw_Application.DTO.Request.AuthRequest;
using FluffyPaw_Application.DTO.Request.EmailRequest;
using FluffyPaw_Application.DTO.Request.StoreManagerRequest;
using FluffyPaw_Application.DTO.Response.AuthResponse;
using FluffyPaw_Application.Services;
using FluffyPaw_Domain.CustomException;
using FluffyPaw_Domain.Entities;
using FluffyPaw_Domain.Enums;
using FluffyPaw_Domain.Interfaces;
using FluffyPaw_Domain.Utils;
using FluffyPaw_Repository.Enum;
using Microsoft.AspNetCore.Http;
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
        private readonly IFilesService _filesService;

        public AuthService(IAuthentication authentication, IUnitOfWork unitOfWork, IMapper mapper, IHashing hashing, IFilesService filesService)
        {
            _authentication = authentication;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _hashing = hashing;
            _filesService = filesService;
        }

        public async Task<bool> RegisterPO(RegisterAccountPORequest registerAccountPORequest)
        {
            var duplicateUsername = _unitOfWork.AccountRepository.Get(du => du.Username.ToLower() == registerAccountPORequest.UserName.ToLower());
            if (duplicateUsername.Any())
            {
                throw new CustomException.DataExistException("Username đã tồn tại.");
            }

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
            account.CreateDate = CoreHelper.SystemTimeNow.AddHours(7);
            account.Status = (int)AccountStatus.Active;
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
            po.Dob = registerAccountPORequest.Dob.AddHours(7);
            po.Reputation = AccountReputation.Good.ToString();
            _unitOfWork.PetOwnerRepository.Insert(po);

            return true;
        }

        public async Task<bool> RegisterSM(RegisterAccountSMRequest registerAccountSMRequest)
        {
            var duplicateUsername = _unitOfWork.AccountRepository.Get(du => du.Username.ToLower() == registerAccountSMRequest.UserName.ToLower());
            if (duplicateUsername.Any())
            {
                throw new CustomException.DataExistException("Username đã tồn tại.");
            }

            if (registerAccountSMRequest.ConfirmPassword != registerAccountSMRequest.Password)
            {
                throw new CustomException.InvalidDataException("Password và ConfirmPassword không trùng khớp.");
            }

            var account = _mapper.Map<Account>(registerAccountSMRequest);
            account.RoleName = RoleName.StoreManager.ToString();
            account.Avatar = "https://cdn-icons-png.flaticon.com/512/10892/10892514.png";
            account.Password = _hashing.SHA512Hash(registerAccountSMRequest.Password);
            account.CreateDate = CoreHelper.SystemTimeNow.AddHours(7);
            account.Status = (int)AccountStatus.Active;
            _unitOfWork.AccountRepository.Insert(account);
            _unitOfWork.Save();

            var identification = _mapper.Map<Identification>(registerAccountSMRequest);
            identification.AccountId = account.Id;
            identification.Front = await _filesService.UploadIdentification(registerAccountSMRequest.Front);
            identification.Back = await _filesService.UploadIdentification(registerAccountSMRequest.Back);
            _unitOfWork.IdentificationRepository.Insert(identification);

            var newWallet = new Wallet
            {
                AccountId = account.Id,
                Balance = 0
            };
            _unitOfWork.WalletRepository.Insert(newWallet);


            var sm = _mapper.Map<Brand>(registerAccountSMRequest);
            sm.AccountId = account.Id;
            sm.Logo = await _filesService.UploadIdentification(registerAccountSMRequest.Logo);
            sm.BusinessLicense = await _filesService.UploadIdentification(registerAccountSMRequest.BusinessLicense);
            sm.Status = false;
            _unitOfWork.BrandRepository.Insert(sm);

            return true;
        }

        public async Task<string> Login(LoginRequest loginRequest)
        {
            string hashedPass = _hashing.SHA512Hash(loginRequest.Password);
            IEnumerable<Account> check = _unitOfWork.AccountRepository.Get(x =>
                x.Username.Equals(loginRequest.Username)
                && x.Password.Equals(hashedPass)
                && !x.RoleName.Equals(RoleName.Admin.ToString()));

            if (!check.Any())
            {
                throw new CustomException.InvalidDataException("Tài khoản hoặc mật khẩu không đúng.");
            }

            Account account = check.First();
            if (account.Status == (int)AccountStatus.Inactive)
            {
                throw new CustomException.InvalidDataException("Tài khoản chưa được kích hoạt.");
            }

            string token = _authentication.GenerateJWTToken(account);
            return token;
        }

        public async Task<string> AdminLogin(LoginRequest loginRequest)
        {
            string hashedPass = _hashing.SHA512Hash(loginRequest.Password);
            IEnumerable<Account> check = _unitOfWork.AccountRepository.Get(x =>
                x.Username.Equals(loginRequest.Username)
                && x.Password.Equals(hashedPass)
            );
            if (!check.Any())
            {
                throw new CustomException.InvalidDataException("Tài khoản hoặc mật khẩu không đúng.");
            }

            Account account = check.First();
            if (!account.RoleName.Equals(RoleName.Admin.ToString()))
            {
                throw new CustomException.InvalidDataException("Tài khoản không có vai trò để đăng nhập.");
            }

            string token = _authentication.GenerateJWTToken(account);
            return token;
        }
    }
}
