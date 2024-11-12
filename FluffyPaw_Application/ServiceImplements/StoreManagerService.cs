 using AutoMapper;
using FluffyPaw_Application.DTO.Request.AuthRequest;
using FluffyPaw_Application.DTO.Request.ServiceRequest;
using FluffyPaw_Application.DTO.Request.StoreManagerRequest;
using FluffyPaw_Application.DTO.Response;
using FluffyPaw_Application.DTO.Response.FilesResponse;
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
using static System.Formats.Asn1.AsnWriter;

namespace FluffyPaw_Application.ServiceImplements
{
    public class StoreManagerService : IStoreManagerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAuthentication _authentication;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IFirebaseConfiguration _firebaseConfiguration;
        private readonly IHashing _hashing;

        public StoreManagerService(IUnitOfWork unitOfWork, IMapper mapper, IAuthentication authentication,
                                    IHttpContextAccessor httpContextAccessor, IFirebaseConfiguration firebaseConfiguration, IHashing hashing)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _authentication = authentication;
            _contextAccessor = httpContextAccessor;
            _firebaseConfiguration = firebaseConfiguration;
            _hashing = hashing;
        }

        public async Task<List<StaffResponse>> GetAllStaffBySM()
        {
            var storemanagerId = _authentication.GetUserIdFromHttpContext(_contextAccessor.HttpContext);
            var account = _unitOfWork.AccountRepository.GetByID(storemanagerId);
            if (account == null)
            {
                throw new CustomException.DataNotFoundException("Không tìm thấy thông tin của StoreManager.");
            }

            var brand = _unitOfWork.BrandRepository.Get(b => b.AccountId == account.Id).FirstOrDefault();
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
                    _mapper.Map(store, staffResponse);
                    staffResponses.Add(staffResponse);
                }

                
            }

            return staffResponses;
        }

        public async Task<List<StoreResponse>> GetAllStoreBySM()
        {
            var storeManagerId = _authentication.GetUserIdFromHttpContext(_contextAccessor.HttpContext);
            var account = _unitOfWork.AccountRepository.GetByID(storeManagerId);
            if (account == null)
            {
                throw new CustomException.DataNotFoundException("Không tìm thấy thông tin của StoreManager.");
            }

            var brand = _unitOfWork.BrandRepository.Get(b => b.AccountId == account.Id).FirstOrDefault();
            if (brand == null)
            {
                throw new CustomException.DataNotFoundException("Không tìm thấy thương hiệu liên kết với tài khoản của bạn.");
            }

            var stores = _unitOfWork.StoreRepository.Get(s => s.BrandId == brand.Id && s.Status == true, includeProperties: "Account");

            if (!stores.Any())
            {
                throw new CustomException.DataNotFoundException("Thương hiệu chưa đăng kí các chi nhánh cửa hàng.");
            }

            var storeResponses = new List<StoreResponse>();

            foreach (var store in stores)
            {
                var storeResponse = _mapper.Map<StoreResponse>(store);

                var storeFiles = _unitOfWork.StoreFileRepository.Get(sf => sf.StoreId == store.Id, includeProperties: "Files")
                                .Select(sf => sf.Files)
                                .ToList();

                storeResponse.Files = _mapper.Map<List<FileResponse>>(storeFiles);

                storeResponses.Add(storeResponse);
            }

            return storeResponses;
        }

        public async Task<List<StoreResponse>> GetAllStoreFalseBySM()
        {
            var storeManagerId = _authentication.GetUserIdFromHttpContext(_contextAccessor.HttpContext);
            var account = _unitOfWork.AccountRepository.GetByID(storeManagerId);
            if (account == null)
            {
                throw new CustomException.DataNotFoundException("Không tìm thấy thông tin của StoreManager.");
            }

            var brand = _unitOfWork.BrandRepository.Get(b => b.AccountId == account.Id).FirstOrDefault();
            if (brand == null)
            {
                throw new CustomException.DataNotFoundException("Không tìm thấy thương hiệu liên kết với tài khoản của bạn.");
            }

            var stores = _unitOfWork.StoreRepository.Get(s => s.BrandId == brand.Id && s.Status == false,
                                            includeProperties: "Account").ToList();
            if (!stores.Any())
            {
                throw new CustomException.DataNotFoundException("Thương hiệu chưa đăng kí các chi nhánh cửa hàng.");
            }

            var storeResponses = new List<StoreResponse>();

            foreach (var store in stores)
            {
                var storeResponse = _mapper.Map<StoreResponse>(store);

                var storeFiles = _unitOfWork.StoreFileRepository.Get(sf => sf.StoreId == store.Id, includeProperties: "Files")
                                .Select(sf => sf.Files)
                                .ToList();

                storeResponse.Files = _mapper.Map<List<FileResponse>>(storeFiles);

                storeResponses.Add(storeResponse);
            }

            return storeResponses;
        }

        public async Task<List<SerResponse>> GetAllServiceFalseBySM()
        {
            var storeManagerId = _authentication.GetUserIdFromHttpContext(_contextAccessor.HttpContext);
            var account = _unitOfWork.AccountRepository.GetByID(storeManagerId);
            if (account == null)
            {
                throw new CustomException.DataNotFoundException("Không tìm thấy thông tin của StoreManager.");
            }

            var brand = _unitOfWork.BrandRepository.Get(b => b.AccountId == account.Id).FirstOrDefault();
            if (brand == null)
            {
                throw new CustomException.DataNotFoundException("Không tìm thấy thương hiệu liên kết với tài khoản của bạn.");
            }

            var services = _unitOfWork.ServiceRepository.Get(s => s.BrandId == brand.Id && s.Status == false,
                                            includeProperties: "ServiceType,Certificate");
            if (!services.Any())
            {
                throw new CustomException.DataNotFoundException("Không tìm thấy dịch vụ nào chưa được xác thực.");
            }

            var serviceResponses = _mapper.Map<List<SerResponse>>(services);

            return serviceResponses;

        }

        public async Task<StoreResponse> CreateStore(StoreRequest storeRequest)
        {
            var storemanagerId = _authentication.GetUserIdFromHttpContext(_contextAccessor.HttpContext);
            var account = _unitOfWork.AccountRepository.GetByID(storemanagerId);
            if (account == null)
            {
                throw new CustomException.DataNotFoundException("Không tìm thấy thông tin của StoreManager.");
            }

            var brand = _unitOfWork.BrandRepository.Get(b => b.AccountId == account.Id).FirstOrDefault();
            if (brand == null)
            {
                throw new CustomException.DataNotFoundException("Thương hiệu của bạn chưa được hệ thống xác thực. Vui lòng thử lại sau");
            }

            var existingUsername = _unitOfWork.AccountRepository.Get(e => e.Username == storeRequest.UserName).ToList();
            if (existingUsername.Any())
            {
                throw new CustomException.DataExistException("Username này đã tồn tại. Vui lòng chọn Username khác.");
            }

            if (storeRequest.ConfirmPassword != storeRequest.Password)
            {
                throw new CustomException.InvalidDataException("Password và ConfirmPassword không trùng khớp.");
            }

            var newStaff = _mapper.Map<Account>(storeRequest);
            newStaff.Password = _hashing.SHA512Hash(storeRequest.Password);
            newStaff.Avatar = "https://cdn-icons-png.flaticon.com/512/10892/10892514.png";
            newStaff.RoleName = RoleName.Staff.ToString();
            newStaff.Status = (int)AccountStatus.Active;
            _unitOfWork.AccountRepository.Insert(newStaff);
            await _unitOfWork.SaveAsync();

            var newStore = _mapper.Map<Store>(storeRequest);
            newStore.AccountId = newStaff.Id;
            newStore.BrandId = brand.Id;
            newStore.OperatingLicense = await _firebaseConfiguration.UploadImage(storeRequest.OperatingLicense);
            newStore.TotalRating = 0f;
            newStore.Status = false;
            _unitOfWork.StoreRepository.Insert(newStore);
            await _unitOfWork.SaveAsync();

            var fileResponses = new List<FileResponse>();

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

                var fileResponse = _mapper.Map<FileResponse>(newFile);
                fileResponses.Add(fileResponse);
            }

            var storeResponse = _mapper.Map<StoreResponse>(newStore);
            storeResponse.Files = fileResponses;

            return storeResponse;
        }

        public async Task<StoreResponse> UpdateStore(long id, UpdateStoreRequest updateStoreRequest)
        {
            var existingstore = _unitOfWork.StoreRepository.GetByID(id);
            if (existingstore == null)
            {
                throw new CustomException.DataNotFoundException("Không tìm thấy chi nhánh.");
            }

            var storeServices = _unitOfWork.StoreServiceRepository.Get(ss => ss.StoreId == id).ToList();

            foreach (var storeService in storeServices)
            {
                var bookings = _unitOfWork.BookingRepository.Get(b => b.StoreServiceId == storeService.Id
                                        && b.Status == BookingStatus.Pending.ToString()
                                        || b.Status == BookingStatus.Accepted.ToString()).ToList();
                if (bookings.Any())
                {
                    throw new CustomException.DataExistException($"Chi nhánh {existingstore.Name} vẫn còn dịch vụ đang được book.");
                }
            }

            _mapper.Map(updateStoreRequest, existingstore);
            existingstore.OperatingLicense = await _firebaseConfiguration.UploadImage(updateStoreRequest.OperatingLicense);
            existingstore.Status = false;
            _unitOfWork.Save();

            var storeFiles = _unitOfWork.StoreFileRepository
                .Get(sf => sf.StoreId == id, includeProperties: "Files")
                .Select(sf => sf.Files)
                .ToList();

            var fileResponses = _mapper.Map<List<FileResponse>>(storeFiles);
            var storeResponse = _mapper.Map<StoreResponse>(existingstore);
            storeResponse.Files = fileResponses;
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
                var bookings = _unitOfWork.BookingRepository.Get(b => b.StoreServiceId == storeService.Id
                                        && b.Status == BookingStatus.Pending.ToString()
                                        || b.Status == BookingStatus.Accepted.ToString()).ToList();
                if (bookings.Any())
                {
                    throw new CustomException.DataExistException($"Chi nhánh {store.Name} vẫn còn dịch vụ đang được book.");
                }
            }

            var staff = _unitOfWork.AccountRepository.GetByID(store.AccountId);

            var storeFiles = _unitOfWork.StoreFileRepository.Get(f => f.StoreId == store.Id).ToList();
            
            foreach ( var storeFile in storeFiles)
            {
                var file = _unitOfWork.FilesRepository.GetByID(storeFile.FileId);
                if (file != null)
                {
                    _unitOfWork.FilesRepository.Delete(file);
                }
                _unitOfWork.StoreFileRepository.Delete(storeFile);
                _unitOfWork.Save();
            }
            _unitOfWork.StoreRepository.Delete(store);
            _unitOfWork.AccountRepository.Delete(staff);

            _unitOfWork.Save();

            return true;
        }

        public async Task<StaffResponse> UpdateStaff(long id, UpdateStaffRequest updateStaffRequest)
        {
            var duplicateUsername = _unitOfWork.AccountRepository.Get(
                                du => du.Username.ToLower() == updateStaffRequest.UserName.ToLower()).ToList();
            if (duplicateUsername.Any())
            {
                throw new CustomException.DataExistException("Username đã tồn tại.");
            }

            if (updateStaffRequest.ConfirmPassword != updateStaffRequest.Password)
            {
                throw new CustomException.InvalidDataException("Password và ConfirmPassword không trùng khớp.");
            }

            var storemanagerId = _authentication.GetUserIdFromHttpContext(_contextAccessor.HttpContext);
            var account = _unitOfWork.AccountRepository.GetByID(storemanagerId);
            if (account == null)
            {
                throw new CustomException.DataNotFoundException("Không tìm thấy thông tin của StoreManager.");
            }

            var storeManagerBrand = _unitOfWork.BrandRepository.Get(b => b.AccountId == account.Id).FirstOrDefault();
            if (storeManagerBrand == null)
            {
                throw new CustomException.DataNotFoundException("Thương hiệu của StoreManager không tồn tại.");
            }

            var existingstaff = _unitOfWork.AccountRepository.GetByID(id);
            if (existingstaff == null)
            {
                throw new CustomException.DataNotFoundException("Không tìm thấy nhân viên.");
            }

            var staffStore = _unitOfWork.StoreRepository.Get(s => s.AccountId == existingstaff.Id).FirstOrDefault();

            if (staffStore.BrandId != storeManagerBrand.Id)
            {
                throw new CustomException.InvalidDataException("Tài khoản này không thuộc quyền quản lý của bạn.");
            }

            _mapper.Map(updateStaffRequest, existingstaff);
            existingstaff.Password = _hashing.SHA512Hash(updateStaffRequest.Password);
            _unitOfWork.Save();

            var staffResponse = _mapper.Map<StaffResponse>(existingstaff);
            return staffResponse;
        }
    }
}
