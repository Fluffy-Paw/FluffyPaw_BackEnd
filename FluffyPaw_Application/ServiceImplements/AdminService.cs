using AutoMapper;
using FluffyPaw_Application.DTO.Request.AdminRequest;
using FluffyPaw_Application.DTO.Request.AuthRequest;
using FluffyPaw_Application.DTO.Response.ServiceResponse;
using FluffyPaw_Application.DTO.Response.BrandResponse;
using FluffyPaw_Application.Services;
using FluffyPaw_Domain.CustomException;
using FluffyPaw_Domain.Entities;
using FluffyPaw_Domain.Enums;
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
            account.Status = (int) AccountStatus.Active;
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

        public IEnumerable<BrandResponse> GetAllBrandFalse()
        {
            var brands = _unitOfWork.BrandRepository.Get().Where(sm => sm.Status == false).ToList();
            var brandResponses = _mapper.Map<IEnumerable<BrandResponse>>(brands);
            return brandResponses;
        }

        public async Task<bool> AcceptBrand(long id)
        {
            var Brand = _unitOfWork.BrandRepository.GetByID(id);
            Brand.Status = true;
            _unitOfWork.Save();
            return true;
        }

        public async Task<List<SerResponse>> GetAllServiceFalseByBrandId(long id)
        {
            var brandService = _unitOfWork.ServiceRepository.Get(ss => ss.BrandId == id && ss.Status == false).ToList();

            if (brandService == null)
            {
                throw new CustomException.DataNotFoundException("Không tìm thấy dịch vụ của doanh nghiệp");
            }

            var serviceResponse = _mapper.Map<List<SerResponse>>(brandService);
            return serviceResponse;
        }

        public async Task<bool> AcceptBrandService(long id)
        {
            var service = _unitOfWork.ServiceRepository.GetByID(id);
            service.Status = true;
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

            if (user.Status == (int)AccountStatus.Active)
            {
                user.Status = (int)AccountStatus.Deactive;
            } else user.Status = (int)AccountStatus.Active;
            _unitOfWork.Save();

            if(user.Status == (int)AccountStatus.Active) return true;
            else return false;
        }

        public async Task<string> DowngradeReputation(long userId)
        {
            var user = _unitOfWork.PetOwnerRepository.Get(po => po.AccountId == userId).FirstOrDefault();
            if(user == null)
            {
                throw new CustomException.DataNotFoundException("Không tìm thấy user.");
            }

            switch(user.Reputation)
            {
                case "Good":
                    user.Reputation = AccountReputation.Warning.ToString();
                    break;

                case "Warning":
                    user.Reputation = AccountReputation.Bad.ToString();
                    break;

                case "Bad":
                    user.Reputation = AccountReputation.Ban.ToString();
                    await ActiveDeactiveAccount(userId);
                    break;

                default:
                    await ActiveDeactiveAccount(userId); 
                    break;
                
            }
            _unitOfWork.Save();

            return user.Reputation;
        }
    }
}
