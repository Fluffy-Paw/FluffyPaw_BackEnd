using AutoMapper;
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

        public async Task<IEnumerable<PetOwner>> GetPetOwners()
        {
            return _unitOfWork.PetOwnerRepository.Get(includeProperties : "Account");
        }

        public async Task<IEnumerable<StaffAddress>> GetStaffAddresses()
        {
            return _unitOfWork.StaffAddressRepository.Get(includeProperties: "Account");
        }

        public async Task<IEnumerable<StoreManager>> GetStoreManagers()
        {
            return _unitOfWork.StoreManagerRepository.Get(includeProperties: "Account");
        }
    }
}
