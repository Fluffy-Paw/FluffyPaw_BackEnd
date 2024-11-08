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
using FluffyPaw_Application.DTO.Response.StoreManagerResponse;
using FluffyPaw_Application.DTO.Response.NotificationResponse;
using FluffyPaw_Application.DTO.Request.NotificationRequest;
using FluffyPaw_Application.DTO.Request.WalletRequest;

namespace FluffyPaw_Application.ServiceImplements
{
    public class AdminService : IAdminService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHashing _hashing;
        private readonly INotificationService _notificationService;
        private readonly IWalletService _walletService;

        public AdminService(IUnitOfWork unitOfWork, IMapper mapper, IHashing hashing, INotificationService notificationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _hashing = hashing;
            _notificationService = notificationService;
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

        public async Task<List<BrandResponse>> GetAllBrandFalse()
        {
            var brands = _unitOfWork.BrandRepository.Get().Where(sm => sm.Status == false).ToList();

            var brandResponses = new List<BrandResponse>();
            foreach (var brand in brands)
            {
                var account = _unitOfWork.AccountRepository.GetByID(brand.AccountId);

                var identification = _unitOfWork.IdentificationRepository.Get(i => i.AccountId == account.Id).FirstOrDefault();

                var brandResponse = _mapper.Map<BrandResponse>(brand);
                brandResponse.CreateDate = account.CreateDate;
                if (identification != null)
                {
                    brandResponse.FullName = identification.FullName;
                    brandResponse.Front = identification.Front;
                    brandResponse.Back = identification.Back;
                }

                brandResponses.Add(brandResponse);
            }

            return brandResponses;
        }

        public async Task<bool> AcceptBrand(long id)
        {
            var Brand = _unitOfWork.BrandRepository.GetByID(id);
            Brand.Status = true;
            _unitOfWork.Save();
            return true;
        }

        public async Task<List<SerResponse>> GetAllServiceFalse()
        {
            var services = _unitOfWork.ServiceRepository.Get(ss => ss.Status == false, includeProperties: "ServiceType,Brand").ToList();

            if (services == null)
            {
                throw new CustomException.DataNotFoundException("Không tìm thấy dịch vụ đợi xác thực.");
            }

            var serviceResponse = _mapper.Map<List<SerResponse>>(services);
            return serviceResponse;
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

        public async Task<List<StoreResponse>> GetAllStoreFalse()
        {
            var stores = _unitOfWork.StoreRepository.Get(ss => ss.Status == false, includeProperties: "Account").ToList();

            if (stores == null)
            {
                throw new CustomException.DataNotFoundException("Không tìm thấy chi nhánh.");
            }

            var storeResponses = _mapper.Map<List<StoreResponse>>(stores);
            return storeResponses;
        }

        public async Task<List<StoreResponse>> GetAllStoreFalseByBrandId(long id)
        {
            var stores = _unitOfWork.StoreRepository.Get(ss => ss.BrandId == id && ss.Status == false, includeProperties: "Account").ToList();

            if (stores == null)
            {
                throw new CustomException.DataNotFoundException("Không tìm thấy chi nhánh của doanh nghiệp");
            }

            var storeResponses = _mapper.Map<List<StoreResponse>>(stores);
            return storeResponses;
        }

        public async Task<bool> AcceptStore(long id)
        {
            var store = _unitOfWork.StoreRepository.GetByID(id);
            store.Status = true;
            _unitOfWork.Save();
            return true;
        }

        public async Task<bool> ActiveInactiveAccount(long userId)
        {
            var user = _unitOfWork.AccountRepository.GetByID(userId);
            if(user == null)
            {
                throw new CustomException.DataNotFoundException("Không tìm thấy người dùng");
            }

            if (user.Status == (int)AccountStatus.Active)
            {
                user.Status = (int)AccountStatus.Inactive;
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
                    await ActiveInactiveAccount(userId);
                    break;

                default:
                    user.Reputation = AccountReputation.Bad.ToString();
                    await ActiveInactiveAccount(userId); 
                    break;
                
            }
            _unitOfWork.Save();

            return user.Reputation;
        }

        public async Task<List<WithdrawNotificationResponse>> GetWithdrawRequest()
        {
            var list = _unitOfWork.NotificationRepository.Get(n => n.Type.Equals("WithDraw Request") && n.ReceiverId == 1).ToList();
            if (!list.Any()) throw new CustomException.DataNotFoundException("Không có yêu cầu rút tiền nào");

            List<WithdrawNotificationResponse> result = new List<WithdrawNotificationResponse>();
            foreach (var item in list)
            {
                var wallet = _unitOfWork.WalletRepository.GetByID(long.Parse(item.Name));
                var request = _mapper.Map<WithdrawNotificationResponse>(item);
                string[] part = item.Description.Split('/');
                request.SenderName = part[0];
                request.amount = double.Parse(part[1]);
                request.number = wallet.Number;
                request.bankName = wallet.BankName;
                request.qr = wallet.QR;

                result.Add(request);
            }

            return result;
        }

        public async Task<bool> CheckoutWithdrawRequest(long id)
        {
            var request = _unitOfWork.NotificationRepository.GetByID(id);
            if (request == null) throw new CustomException.DataNotFoundException("Không tìm thấy yêu cầu.");

            request.Status = "Completed";
            await _unitOfWork.SaveAsync();
            return true;
        }

        public async Task<bool> DenyWithdrawRequest(DenyWithdrawRequest denyWithdrawRequest)
        {
            var request = _unitOfWork.NotificationRepository.GetByID(denyWithdrawRequest.Id);
            if (request == null) throw new CustomException.DataNotFoundException("Không tìm thấy yêu cầu.");

            string[] part = request.Description.Split('/');

            var user = _unitOfWork.AccountRepository.Get(u => u.Username.Equals(part[0])).FirstOrDefault();
            if (user == null) throw new CustomException.DataNotFoundException($"Không tìm thấy tài khoản.");

            var wallet = _unitOfWork.WalletRepository.Get(w => w.AccountId.Equals(user.Id)).FirstOrDefault();

            await _notificationService.CreateNotification(new NotificationRequest
            {
                Name = user.Username,
                ReceiverId = user.Id,
                Type = "Withdraw Request",
                Description = denyWithdrawRequest.Description
            });

            request.Status = "Denied";
            
            wallet.Balance += double.Parse(part[1]);

            await _unitOfWork.SaveAsync();

            return true;
        }
    }
}
