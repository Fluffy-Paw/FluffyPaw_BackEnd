using AutoMapper;
using FluffyPaw_Application.DTO.Request.ServiceRequest;
using FluffyPaw_Application.DTO.Request.StoreManagerRequest;
using FluffyPaw_Application.DTO.Response;
using FluffyPaw_Application.DTO.Response.ServiceResponse;
using FluffyPaw_Application.DTO.Response.StoreManagerResponse;
using FluffyPaw_Application.Services;
using FluffyPaw_Domain.CustomException;
using FluffyPaw_Domain.Entities;
using FluffyPaw_Domain.Enums;
using FluffyPaw_Domain.Interfaces;
using FluffyPaw_Repository.Enum;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Application.ServiceImplements
{
    public class StoreManagerService : IStoreManagerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAuthentication _authentication;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IFirebaseConfiguration _firebaseConfiguration;

        public StoreManagerService(IUnitOfWork unitOfWork, IMapper mapper, IAuthentication authentication,
                                    IHttpContextAccessor httpContextAccessor, IFirebaseConfiguration firebaseConfiguration)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _authentication = authentication;
            _contextAccessor = httpContextAccessor;
            _firebaseConfiguration = firebaseConfiguration;
        }

        public async Task<List<StaffResponse>> GetAllStaffBySM()
        {
            var storemanagerId = _authentication.GetUserIdFromHttpContext(_contextAccessor.HttpContext);
            var account = _unitOfWork.AccountRepository.GetByID(storemanagerId);
            if (account == null)
            {
                throw new CustomException.DataNotFoundException("Không tìm thấy thông tin của StoreManager.");
            }

            var brand = _unitOfWork.BrandRepository.Get(b => b.AccountId == account.Id).First();
            var stores = _unitOfWork.StoreRepository.Get(s => s.BrandId == brand.Id).ToList();
            if (stores == null || stores.Count == 0)
            {
                throw new CustomException.DataNotFoundException("Thương hiệu chưa đăng kí các chi nhánh cửa hàng.");
            }

            var staffResponses = new List<StaffResponse>();
            foreach (var store in stores)
            {
                var staff = _unitOfWork.AccountRepository.GetByID(store.AccountId);
                if (staff != null)
                {
                    var staffResponse = _mapper.Map<StaffResponse>(staff);
                    staffResponses.Add(staffResponse);
                }
            }

            return staffResponses;
        }

        public async Task<List<StoreResponse>> GetAllStoreBySM()
        {
            var storemanagerId = _authentication.GetUserIdFromHttpContext(_contextAccessor.HttpContext);
            var account = _unitOfWork.AccountRepository.GetByID(storemanagerId);
            if (account == null)
            {
                throw new CustomException.DataNotFoundException("Không tìm thấy thông tin của StoreManager.");
            }

            var brand = _unitOfWork.BrandRepository.Get(b => b.AccountId == account.Id).First();
            var stores = _unitOfWork.StoreRepository.Get(s => s.BrandId == brand.Id && s.Status == true).ToList();
            if (stores == null || stores.Count == 0)
            {
                throw new CustomException.DataNotFoundException("Thương hiệu chưa đăng kí các chi nhánh cửa hàng.");
            }

            var storeResponses = new List<StoreResponse>();
            foreach (var store in stores)
            {
                var storeResponse = _mapper.Map<StoreResponse>(store);
                storeResponses.Add(storeResponse);
            }

            return storeResponses;
        }

        public async Task<List<StoreResponse>> GetAllStoreFalseBySM()
        {
            var storemanagerId = _authentication.GetUserIdFromHttpContext(_contextAccessor.HttpContext);
            var account = _unitOfWork.AccountRepository.GetByID(storemanagerId);
            if (account == null)
            {
                throw new CustomException.DataNotFoundException("Không tìm thấy thông tin của StoreManager.");
            }

            var brand = _unitOfWork.BrandRepository.Get(b => b.AccountId == account.Id).First();
            var stores = _unitOfWork.StoreRepository.Get(s => s.BrandId == brand.Id && s.Status == false).ToList();
            if (stores == null || stores.Count == 0)
            {
                throw new CustomException.DataNotFoundException("Thương hiệu chưa đăng kí các chi nhánh cửa hàng.");
            }

            var storeResponses = new List<StoreResponse>();
            foreach (var store in stores)
            {
                var storeResponse = _mapper.Map<StoreResponse>(store);
                storeResponses.Add(storeResponse);
            }

            return storeResponses;
        }

        public async Task<StoreResponse> CreateStore(StoreRequest storeRequest)
        {
            var storemanagerId = _authentication.GetUserIdFromHttpContext(_contextAccessor.HttpContext);
            var account = _unitOfWork.AccountRepository.GetByID(storemanagerId);
            if (account == null)
            {
                throw new CustomException.DataNotFoundException("Không tìm thấy thông tin của StoreManager.");
            }

            var brand = _unitOfWork.BrandRepository.Get(b => b.AccountId == account.Id).First();
            if (brand == null)
            {
                throw new CustomException.DataNotFoundException("Thương hiệu của bạn chưa được hệ thống xác thực. Vui lòng thử lại sau");
            }

            var existingUsername = _unitOfWork.AccountRepository.Get(e => e.Username == storeRequest.UserName).ToList();
            if (existingUsername.Any())
            {
                throw new CustomException.DataExistException("Username này đã tồn tại. Vui lòng chọn Username khác.");
            }

            var newStaff = _mapper.Map<Account>(storeRequest);
            newStaff.Avatar = "https://cdn-icons-png.flaticon.com/512/10892/10892514.png";
            newStaff.RoleName = RoleName.Staff.ToString();
            newStaff.Status = (int)AccountStatus.Active;
            _unitOfWork.AccountRepository.Insert(newStaff);
            await _unitOfWork.SaveAsync();

            var newStore = _mapper.Map<Store>(storeRequest);
            newStore.AccountId = newStaff.Id;
            newStore.BrandId = brand.Id;
            newStore.TotalRating = 0f;
            newStore.Status = false;
            _unitOfWork.StoreRepository.Insert(newStore);
            await _unitOfWork.SaveAsync();

            foreach (var file in storeRequest.File)
            {
                var newFile = new Files
                {
                    File = await _firebaseConfiguration.UploadImage(file),
                    Status = true
                };
                _unitOfWork.FilesRepository.Insert(newFile);
                await _unitOfWork.SaveAsync();

                var newStoreFile = new StoreFile
                {
                    FileId = newFile.Id,
                    StoreId = newStore.Id,
                };
                _unitOfWork.StoreFileRepository.Insert(newStoreFile);
                _unitOfWork.Save();
            }

            var storeResponse = _mapper.Map<StoreResponse>(newStore);

            return storeResponse;
        }

        public async Task<StoreResponse> UpdateStore(long id, UpdateStoreRequest updateStoreRequest)
        {
            var existingstore = _unitOfWork.StoreRepository.GetByID(id);
            if (existingstore == null)
            {
                throw new CustomException.DataNotFoundException("Không tìm thấy chi nhánh.");
            }

            _mapper.Map(updateStoreRequest, existingstore);
            _unitOfWork.Save();

            var storeResponse = _mapper.Map<StoreResponse>(existingstore);
            return storeResponse;
        }

        public async Task<bool> DeleteStore(long id)
        {
            var store = _unitOfWork.StoreRepository.GetByID(id);
            if (store == null)
            {
                throw new CustomException.DataNotFoundException("Không tìm thấy chi nhánh.");
            }

            var storeServices = _unitOfWork.StoreServiceRepository.Get(ss => ss.StoreId == store.Id).ToList();

            foreach (var storeService in storeServices)
            {
                var bookings = _unitOfWork.BookingRepository.Get(b => b.StoreServiceId == storeService.Id).ToList();
                if (bookings.Any())
                {
                    throw new CustomException.DataExistException($"Chi nhánh {store.Name} vẫn còn dịch vụ đang được book.");
                }
            }
            _unitOfWork.StoreRepository.Delete(store);


            _unitOfWork.Save();

            return true;
        }
    }
}
