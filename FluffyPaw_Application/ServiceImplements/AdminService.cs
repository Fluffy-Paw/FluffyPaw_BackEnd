using AutoMapper;
using FluffyPaw_Application.DTO.Request.AdminRequest;
using FluffyPaw_Application.DTO.Request.AuthRequest;
using FluffyPaw_Application.DTO.Response.ServiceTypeResponse;
using FluffyPaw_Application.DTO.Response.StoreManagerResponse;
using FluffyPaw_Application.Services;
using FluffyPaw_Domain.CustomException;
using FluffyPaw_Domain.Entities;
using FluffyPaw_Domain.Interfaces;
using FluffyPaw_Repository.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Application.ServiceImplements
{
    public class AdminService : IAdminService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHashing _hashing;

        public AdminService(IUnitOfWork unitOfWork, IMapper mapper, IHashing hashing)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _hashing = hashing;
        }

        public async Task<bool> CreateAdmin(AdminRequest adminRequest)
        {
            if (adminRequest.ComfirmPassword != adminRequest.Password)
            {
                throw new CustomException.InvalidDataException("Password và ConfirmPassword không trùng khớp.");
            }

            var account = _mapper.Map<Account>(adminRequest);
            account.RoleName = RoleName.Admin.ToString();
            account.Avatar = "https://cdn-icons-png.flaticon.com/512/10892/10892514.png";
            account.Password = _hashing.SHA512Hash(adminRequest.Password);
            account.Status = true;
            _unitOfWork.AccountRepository.Insert(account);
            _unitOfWork.Save();

            var wallet = new Wallet
            {
                AccountId = account.Id,
                Balance = 0
            };
            _unitOfWork.WalletRepository.Insert(wallet);

            return true;
        }

        public IEnumerable<StoreManagerResponse> GetAllStoreManagerFalse()
        {
            var storeManagers = _unitOfWork.StoreManagerRepository.Get().Where(sm => sm.Status == false).ToList();
            var storeManagerResponses = _mapper.Map<IEnumerable<StoreManagerResponse>>(storeManagers);
            return storeManagerResponses;
        }

        public async Task<bool> AcceptStoreManager(long id)
        {
            var storeManager = _unitOfWork.StoreManagerRepository.GetByID(id);
            storeManager.Status = true;
            _unitOfWork.Save();
            return true;
        }

        public async Task<bool> ActiveDeactiveAccount(long userId)
        {
            var user = _unitOfWork.AccountRepository.GetByID(userId);
            if(user == null)
            {
                throw new CustomException.DataNotFoundException("Không tìm thấy người dùng");
            }

            if (user.Status == true)
            {
                user.Status = false;
            } else user.Status = true;
            _unitOfWork.Save();

            return user.Status;
        }

        public async Task<IEnumerable<Account>> GetAllAccounts()
        {
            return _unitOfWork.AccountRepository.Get(orderBy: ob => ob.OrderByDescending(a => a.RoleName));
        }
    }
}
