using AutoMapper;
using FluffyPaw_Application.DTO.Response;
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
    public class AccountService : IAccountService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthentication _authentication;
        private readonly IHashing _hashing;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AccountService(IMapper mapper, IUnitOfWork unitOfWork, IAuthentication authentication, IHashing hashing, IHttpContextAccessor httpContextAccessor)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _authentication = authentication;
            _hashing = hashing;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<bool> ChangePassword(string oldPassword, string newPassword)
        {
            var accountId = _authentication.GetUserIdFromHttpContext(_httpContextAccessor.HttpContext);
            var user = _unitOfWork.AccountRepository.GetByID(accountId);

            if (user == null)
            {
                throw new CustomException.DataNotFoundException("Không tìm thấy user.");
            }

            if (_hashing.SHA512Hash(oldPassword) != user.Password)
            {
                throw new CustomException.InvalidDataException("Sai mật khẩu cũ.");
            }

            if (oldPassword == newPassword)
            {
                throw new CustomException.InvalidDataException("Mật khẩu mới trùng với mật khẩu cũ");
            }
            
            if (newPassword.Length < 8)
            {
                throw new CustomException.InvalidDataException("Vui lòng nhập mật khẩu tối thiểu 8 ký tự.");
            }

            user.Password = _hashing.SHA512Hash(newPassword);
            _unitOfWork.Save();

            return true;
        }

        public async Task<IEnumerable<AccountResponse>> GetPetOwners()
        {
            var user = _unitOfWork.PetOwnerRepository.Get(includeProperties: "Account");
            List<AccountResponse> result = new List<AccountResponse>();

            foreach (var account in user)
            {
                var temp = new AccountResponse
                {
                    UserId = account.Account.Id,
                    Username = account.Account.Username,
                    Fullname = account.FullName,
                    Email = account.Account.Email,
                    Phone = account.Phone,
                    Dob = account.Dob,
                    Reputation = account.Reputation,
                    Status = account.Account.Status
                };
                result.Add(temp);
            }
            return result;
        }

        public async Task<IEnumerable<AccountResponse>> GetStores()
        {
            var user = _unitOfWork.StaffAddressRepository.Get(includeProperties: "Account");
            List<AccountResponse> result = new List<AccountResponse>();

            foreach (var account in user)
            {
                var temp = new AccountResponse
                {
                    UserId = account.Account.Id,
                    Username = account.Account.Username,
                    Fullname = account.Name,
                    Email = account.Account.Email,
                    Phone = account.Phone,
                    Status = account.Account.Status
                };
                result.Add(temp);
            }
            return result;
        }

        public async Task<IEnumerable<AccountResponse>> GetStoreManagers()
        {
            var user = _unitOfWork.StoreManagerRepository.Get(includeProperties: "Account");
            List<AccountResponse> result = new List<AccountResponse>();

            foreach (var account in user)
            {
                var temp = new AccountResponse
                {
                    UserId = account.Account.Id,
                    Username = account.Account.Username,
                    Fullname = account.Name,
                    Email = account.Account.Email,
                    Status = account.Account.Status
                };
                result.Add(temp);
            }
            return result;
        }

        public async Task<IEnumerable<Account>> GetAllAccounts()
        {
            return _unitOfWork.AccountRepository.Get(orderBy: ob => ob.OrderByDescending(a => a.RoleName));
        }
    }
}
