using AutoMapper;
using FluffyPaw_Application.DTO.Response;
using FluffyPaw_Application.Services;
using FluffyPaw_Domain.Entities;
using FluffyPaw_Domain.Interfaces;
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

        public AccountService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        //Email CreateDate Username
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
                    Reputation = account.Status,
                    Status = account.Account.Status
                };
                result.Add(temp);
            }
            return result;
        }

        public async Task<IEnumerable<AccountResponse>> GetStaffAddresses()
        {
            var user = _unitOfWork.StaffAddressRepository.Get(includeProperties: "Account");
            List<AccountResponse> result = new List<AccountResponse>();

            foreach (var account in user)
            {
                var temp = new AccountResponse
                {
                    UserId = account.Account.Id,
                    Username = account.Account.Username,
                    Fullname = account.StaffAddressName,
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
    }
}
