﻿using AutoMapper;
using FluffyPaw_Application.DTO.Request.PetRequest;
using FluffyPaw_Application.DTO.Response.PetResponse;
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
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Application.ServiceImplements
{
    public class PetService : IPetService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IFirebaseConfiguration _firebaseConfiguration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAuthentication _authentication;
        private readonly IHashing _hashing;

        public PetService(IUnitOfWork unitOfWork, IMapper mapper, IFirebaseConfiguration firebaseConfiguration, IHttpContextAccessor httpContextAccessor, IAuthentication authentication, IHashing hashing)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _firebaseConfiguration = firebaseConfiguration;
            _httpContextAccessor = httpContextAccessor;
            _authentication = authentication;
            _hashing = hashing;
        }

        public async Task<bool> ActiveDeactivePet(long petId)
        {
            bool result;
            var existingPet = _unitOfWork.PetRepository.GetByID(petId);
            if (existingPet == null)
            {
                throw new CustomException.DataNotFoundException("Không tìm thấy thú cưng.");
            }
            if(existingPet.Status == PetStatus.Available.ToString())
            { 
                existingPet.Status = PetStatus.Unavailable.ToString();
                result = false;
            }
            else
            {
                existingPet.Status = PetStatus.Available.ToString();
                result = true;
            }
            _unitOfWork.Save();

            return result;
        }

        public async Task<long> AddBehavior(string Action)
        {
            var existingBehavior = _unitOfWork.BehaviorCategoryRepository.Get(b => b.Name.ToLower() == Action.ToLower()).FirstOrDefault();

            if (existingBehavior != null)
            {
                return existingBehavior.Id;
            }

            _unitOfWork.BehaviorCategoryRepository.Insert(new BehaviorCategory { Name = Action});
            _unitOfWork.Save();

            return _unitOfWork.BehaviorCategoryRepository.Get(b => b.Name.ToLower() == Action.ToLower()).FirstOrDefault().Id;
        }

        public async Task<long> AddPetType(PetTypeRequest petTypeRequest)
        {
            var petTypes = _unitOfWork.PetTypeRepository.Get().ToList();
            var existing = petTypes.Where(t => t.Name.Equals(petTypeRequest.Name)).FirstOrDefault();
            if (existing != null)
            {
                return existing.Id;
            }
            var newPetType = _mapper.Map<PetType>(petTypeRequest);
            newPetType.Image = "";
            _unitOfWork.PetTypeRepository.Insert(newPetType);
            return petTypes.Count + 1;
        }

        public async Task<bool> ChangeOwnerOfPet(ChangePORequest changePORequest)
        {
            var accountId = _authentication.GetUserIdFromHttpContext(_httpContextAccessor.HttpContext);
            var user = _unitOfWork.PetOwnerRepository.Get(u => u.Account.Username == changePORequest.NewOwnerUsername, includeProperties: "Account").FirstOrDefault();
            var po = _unitOfWork.PetOwnerRepository.Get(u => u.AccountId.Equals(accountId), includeProperties: "Account").FirstOrDefault();
            var pet = _unitOfWork.PetRepository.GetByID(changePORequest.PetId);
            var petList = _unitOfWork.PetRepository.Get(p => p.PetOwnerId.Equals(user));
            var booking = _unitOfWork.BookingRepository.Get(b => b.PetId.Equals(pet.Id) && b.Checkin == false && b.CheckOut == false).ToList();

            if (user == null) throw new CustomException.DataNotFoundException($"Không tìm thấy người dùng với tài khoản: {changePORequest.NewOwnerUsername}");
            if (user.Account.RoleName != RoleName.PetOwner.ToString()) throw new CustomException.ForbbidenException("User không phải Pet Owner.");
            if (pet == null) throw new CustomException.DataNotFoundException("Không tìm thấy thú cưng");
            if (user.Id.Equals(po.Id)) throw new CustomException.InvalidDataException("Bạn đang chuyển quyền nuôi dưỡng cho chính bản thân, hãy nhập Tên Tài Khoản của đối tượng cần chuyển.");
            if (pet.PetOwnerId != po.Id) throw new CustomException.ForbbidenException("Bạn không thể chuyển quyền nuôi dưỡng vì đây không phải thú cưng của bạn.");
            if (petList.Count() == 5) throw new CustomException.DataExistException($"Tài khoản {changePORequest.NewOwnerUsername} đã đạt số lượng tối đa 5 thú cưng.");
            if (_hashing.SHA512Hash(changePORequest.YourPassword) != po.Account.Password) throw new CustomException.ForbbidenException("Bạn đã nhập sai mật khẩu.");
            if (user.Account.Status == (int)AccountStatus.Inactive) throw new CustomException.ForbbidenException($"Tài khoản {changePORequest.NewOwnerUsername} đã bị cấm.");
            if (booking.Any()) throw new CustomException.ForbbidenException("Thú cưng này đang có lịch đặt, không thể chuyển chủ nhân !");

            pet.PetOwnerId = user.Id;
            _unitOfWork.Save();

            return true;
        }

        public async Task<bool> CreateNewPet(PetRequest petRequest)
        {
            var accountId = _authentication.GetUserIdFromHttpContext(_httpContextAccessor.HttpContext);
            var po = _unitOfWork.PetOwnerRepository.Get(u => u.AccountId == accountId && u.Account.Status == (int)AccountStatus.Active, includeProperties: "Account").FirstOrDefault();
            if (po == null)
            {
                throw new CustomException.DataNotFoundException("Không tìm thấy user.");
            }

            var existingPet = _unitOfWork.PetRepository.Get(p=>p.PetOwnerId == po.Id && p.Status != PetStatus.Deleted.ToString());
            if (existingPet.Count() >= 5)
            {
                throw new CustomException.InvalidDataException("Bạn chỉ được lưu tối đa 5 thú cưng.");
            }

            if(petRequest.MicrochipNumber != null)
            {
                var duplicatePet = _unitOfWork.PetRepository.Get(m => m.MicrochipNumber == petRequest.MicrochipNumber).FirstOrDefault();
                if(duplicatePet != null)
                {
                    throw new CustomException.InvalidDataException($"Đã tồn tại pet với số microchip {petRequest.MicrochipNumber}");
                }
            }

            if (petRequest.Dob > CoreHelper.SystemTimeNow) throw new CustomException.InvalidDataException("Ngày sinh không được lớn hơn ngày hiện tại.");

            var pet = _mapper.Map<Pet>(petRequest);
            pet.PetOwnerId = po.Id;
            if (petRequest.PetImage != null) pet.Image = await _firebaseConfiguration.UploadImage(petRequest.PetImage);
            pet.Dob = petRequest.Dob.AddHours(7);
            pet.Status = PetStatus.Available.ToString();
            _unitOfWork.PetRepository.Insert(pet);
            _unitOfWork.Save();

            return true;
        }

        public async Task<bool> DeleteBehavior(long id)
        {
            var existingBehavior = _unitOfWork.BehaviorCategoryRepository.Get(b => b.Id == id).FirstOrDefault();

            if (existingBehavior == null) throw new CustomException.DataNotFoundException("Không tìm thấy hành vi cần xóa");

            var existingPet = _unitOfWork.PetRepository.Get(p => p.BehaviorCategoryId == id);
            if (existingPet.Any())
            {
                foreach (var pet in existingPet)
                {
                    pet.BehaviorCategoryId = 1;
                }
            }

            _unitOfWork.BehaviorCategoryRepository.Delete(existingBehavior);
            await _unitOfWork.SaveAsync();

            return true;
        }

        public async Task<bool> DeletePet(long petId)
        {
            var existingPet = _unitOfWork.PetRepository.GetByID(petId);
            if (existingPet == null)
            {
                throw new CustomException.DataNotFoundException("Không tìm thấy thú cưng.");
            }
            var booking = _unitOfWork.BookingRepository.Get(b => b.PetId.Equals(petId) && b.Checkin == false && b.CheckOut == false).ToList();
            if (booking.Any()) throw new CustomException.ForbbidenException("Thú cưng này đang có lịch đặt, không thể xóa !");

            existingPet.Status = PetStatus.Deleted.ToString();
            await _unitOfWork.SaveAsync();

            return true;
        }

        public async Task<IEnumerable<BehaviorCategory>> GetAllBehavior()
        {
            return _unitOfWork.BehaviorCategoryRepository.GetAll();
        }

        public async Task<IEnumerable<ListPetResponse>> GetAllPetOfUser()
        {
            var accountId = _authentication.GetUserIdFromHttpContext(_httpContextAccessor.HttpContext);
            var po = _unitOfWork.PetOwnerRepository.Get(u => u.AccountId == accountId && u.Account.Status == (int)AccountStatus.Active, includeProperties: "Account").FirstOrDefault();
            if (po == null)
            {
                throw new CustomException.DataNotFoundException("Không tìm thấy user.");
            }
            if (po.Reputation == AccountReputation.Ban.ToString())
            {
                throw new CustomException.ForbbidenException("Bạn đã bị cấm.");
            }
            var pet = _unitOfWork.PetRepository.Get(p => p.PetOwnerId == po.Id && p.Status != PetStatus.Deleted.ToString(), includeProperties: "BehaviorCategory,PetType.PetCategory");
            if (!pet.Any())
            {
                throw new CustomException.DataNotFoundException("Bạn chưa nhập thông tin thú cưng.");
            }

            var result = new List<ListPetResponse>();
            foreach (var item in pet)
            {
                var p = _mapper.Map<ListPetResponse>(item);
                p.PetCategoryId = item.PetType.PetCategoryId;
                p.BehaviorCategory = item.BehaviorCategory.Name;
                result.Add(p);
            }

            return result;
        }

        public async Task<IEnumerable<PetType>> GetAllPetType(long categoryId)
        {
            var result = _unitOfWork.PetTypeRepository.Get(c => c.PetCategoryId == categoryId, includeProperties: "PetCategory");
            
            if(!result.Any())
            {
                throw new CustomException.DataNotFoundException("Không tìm thấy giống loài của chủng loài này.");
            }
            
            return result;
        }

        public async Task<BehaviorCategory> GetBehavior(long behaviorId)
        {
            var behavior = _unitOfWork.BehaviorCategoryRepository.GetByID(behaviorId);

            if (behavior == null) throw new CustomException.DataNotFoundException($"Không tìm thấy hành vi có id {behaviorId}");

            return behavior;
        }

        public async Task<PetResponse> GetPet(long petId)
        {
            var pet = _unitOfWork.PetRepository.Get(p => p.Id == petId, includeProperties : "BehaviorCategory,PetType.PetCategory,PetType").FirstOrDefault();
            if (pet == null)
            {
                throw new CustomException.DataNotFoundException("Không tìm thấy thú cưng.");
            }
            if(pet.Status == PetStatus.Deleted.ToString())
            {
                throw new CustomException.InvalidDataException("Bạn đã xóa thú cưng này.");
            }

            var result = _mapper.Map<PetResponse>(pet);
            result.PetCategoryId = pet.PetType.PetCategoryId;

            int year = DateTimeOffset.Now.Year - pet.Dob.Year;
            int month = DateTimeOffset.Now.Month - pet.Dob.Month;
            
            if (month < 0)
            {
                year--;
                month += 12;
            }
            if (year == 0)
            {
                if (DateTimeOffset.Now.Month == pet.Dob.Month) result.Age = (DateTimeOffset.Now.Day - pet.Dob.Day).ToString() + " ngày";
                else result.Age = month.ToString() + " tháng";
            }
            else
            {
                result.Age = year.ToString() + " năm " + month.ToString() + " tháng";
            }

            return result;
        }

        public async Task<PetType> GetPetType(long petTypeId)
        {
            var petType = _unitOfWork.PetTypeRepository.GetByID(petTypeId);

            if (petType == null) throw new CustomException.DataNotFoundException("Không tìm thấy giống loài.");

            return petType;
        }

        public async Task<PetResponse> UpdatePet(long petId, PetRequest petRequest)
        {
            var pet = _unitOfWork.PetRepository.Get(p => p.Id == petId, includeProperties: "BehaviorCategory,PetType.PetCategory,PetType").FirstOrDefault();
            if (pet == null)
            {
                throw new CustomException.DataNotFoundException("Không tìm thấy thú cưng.");
            }
            if (petRequest.MicrochipNumber != pet.MicrochipNumber && petRequest.MicrochipNumber != "none")
            {
                var duplicatePet = _unitOfWork.PetRepository.Get(m => m.MicrochipNumber == petRequest.MicrochipNumber).FirstOrDefault();
                if (duplicatePet != null)
                {
                    throw new CustomException.InvalidDataException($"Đã tồn tại pet với số microchip {petRequest.MicrochipNumber}");
                }
            }

            if (petRequest.Dob > DateTimeOffset.UtcNow) throw new CustomException.InvalidDataException("Ngày sinh không hợp lệ.");

            if (petRequest.PetImage != null) pet.Image = await _firebaseConfiguration.UploadImage(petRequest.PetImage);
            _mapper.Map(petRequest, pet);
            pet.Dob = petRequest.Dob.AddHours(7);
            _unitOfWork.Save();

            return await GetPet(petId);
        }
    }
}
