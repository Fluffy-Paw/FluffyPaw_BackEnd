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
using static System.Formats.Asn1.AsnWriter;

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
                throw new CustomException.DataNotFoundException("Không tìm thấy stores.");
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
            if (!user.Any()) throw new CustomException.DataNotFoundException("Không tìm thấy Pet Owner nào.");
            List<AccountResponse> result = new List<AccountResponse>();

            foreach (var account in user)
            {
                var temp = _mapper.Map<AccountResponse>(account);
                temp.Username = account.Account.Username;
                temp.Email = account.Account.Email;
                temp.Status = account.Account.Status;

                result.Add(temp);
            }
            return result;
        }

        public async Task<IEnumerable<AccountResponse>> GetStores()
        {
            var stores = _unitOfWork.StoreRepository.Get(includeProperties: "Account");
            if (!stores.Any()) throw new CustomException.DataNotFoundException("Không tìm thấy Store nào.");
            List<AccountResponse> result = new List<AccountResponse>();

            foreach (var store in stores)
            {
                var temp = _mapper.Map<AccountResponse>(store);
                temp.Username = store.Account.Username;
                temp.Email = store.Account.Email;
                temp.Status = store.Account.Status;
                temp.Fullname = store.Name;

                result.Add(temp);
            }

            return result;
        }

        public async Task<IEnumerable<AccountResponse>> GetBrands()
        {
            var brands = _unitOfWork.BrandRepository.Get(includeProperties: "Account");
            if (!brands.Any()) throw new CustomException.DataNotFoundException("Không tìm thấy Brand nào.");
            List<AccountResponse> result = new List<AccountResponse>();

            foreach (var brand in brands)
            {
                var temp = _mapper.Map<AccountResponse>(brand);
                temp.Fullname = brand.Name;
                temp.Username = brand.Account.Username;
                temp.Email = brand.Account.Email;
                temp.Phone = brand.Hotline;

                result.Add(temp);
            }

            return result;
        }

        //public async Task<IEnumerable<Account>> GetAllAccounts()
        //{
        //    return _unitOfWork.AccountRepository.Get(orderBy: ob => ob.OrderByDescending(a => a.RoleName));
        //}

        public async Task<IEnumerable<AccountResponse>> GetStoresByBrandId(long id)
        {
            var stores = _unitOfWork.StoreRepository.Get(s => s.BrandId == id, includeProperties: "Account,Brand");
            if (!stores.Any()) throw new CustomException.DataNotFoundException("Không tìm thấy Store nào.");
            List<AccountResponse> result = new List<AccountResponse>();

            foreach (var store in stores)
            {
                var temp = _mapper.Map<AccountResponse>(store);
                temp.Username = store.Account.Username;
                temp.Email = store.Account.Email;
                temp.Status = store.Account.Status;
                temp.Fullname = store.Name;

                result.Add(temp);
            }

            return result;
        }
    }
}
