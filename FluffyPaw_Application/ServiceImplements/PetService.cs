using AutoMapper;
using FluffyPaw_Application.DTO.Request.PetRequest;
using FluffyPaw_Application.DTO.Response.PetResponse;
using FluffyPaw_Application.Services;
using FluffyPaw_Domain.CustomException;
using FluffyPaw_Domain.Entities;
using FluffyPaw_Domain.Enums;
using FluffyPaw_Domain.Interfaces;
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

        public PetService(IUnitOfWork unitOfWork, IMapper mapper, IFirebaseConfiguration firebaseConfiguration, IHttpContextAccessor httpContextAccessor, IAuthentication authentication)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _firebaseConfiguration = firebaseConfiguration;
            _httpContextAccessor = httpContextAccessor;
            _authentication = authentication;
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

            if(petRequest.MicrochipNumber != "none")
            {
                var duplicatePet = _unitOfWork.PetRepository.Get(m => m.MicrochipNumber == petRequest.MicrochipNumber).FirstOrDefault();
                if(duplicatePet != null)
                {
                    throw new CustomException.InvalidDataException($"Đã tồn tại pet với số microchip {petRequest.MicrochipNumber}");
                }
            }

            if (petRequest.Dob > DateTimeOffset.UtcNow) throw new CustomException.InvalidDataException("Ngày sinh không hợp lệ.");

            var pet = _mapper.Map<Pet>(petRequest);
            pet.PetOwnerId = po.Id;
            if (petRequest.Image != null) pet.Image = await _firebaseConfiguration.UploadImage(petRequest.Image);
            
            pet.Status = PetStatus.Available.ToString();
            _unitOfWork.PetRepository.Insert(pet);
            _unitOfWork.Save();

            return true;
        }

        public async Task<bool> DeletePet(long petId)
        {
            var existingPet = _unitOfWork.PetRepository.GetByID(petId);
            if (existingPet == null)
            {
                throw new CustomException.DataNotFoundException("Không tìm thấy thú cưng.");
            }
            existingPet.Status = PetStatus.Deleted.ToString();
            _unitOfWork.Save();

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
            result.Age = DateTime.Now.Year - pet.Dob.Year;
            if (DateTime.Now.Month < pet.Dob.Month || (DateTime.Now.Month == pet.Dob.Month && DateTime.Now.Day < pet.Dob.Day))
            {
                result.Age--;
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
            if (petRequest.MicrochipNumber != pet.MicrochipNumber || petRequest.MicrochipNumber != "none")
            {
                var duplicatePet = _unitOfWork.PetRepository.Get(m => m.MicrochipNumber == petRequest.MicrochipNumber).FirstOrDefault();
                if (duplicatePet != null)
                {
                    throw new CustomException.InvalidDataException($"Đã tồn tại pet với số microchip {petRequest.MicrochipNumber}");
                }
            }

            if (petRequest.Dob > DateTimeOffset.UtcNow) throw new CustomException.InvalidDataException("Ngày sinh không hợp lệ.");

            if (petRequest.Image != null) pet.Image = await _firebaseConfiguration.UploadImage(petRequest.Image);
            _mapper.Map(petRequest, pet);
            _unitOfWork.Save();

            var result = _mapper.Map<PetResponse>(pet);
            result.PetCategoryId = _unitOfWork.PetTypeRepository.GetByID(pet.PetTypeId).PetCategoryId;
            result.BehaviorCategory = await GetBehavior(pet.BehaviorCategoryId);
            result.Age = DateTime.Now.Year - pet.Dob.Year;
            if (DateTime.Now.Month < pet.Dob.Month || (DateTime.Now.Month == pet.Dob.Month && DateTime.Now.Day < pet.Dob.Day))
            {
                result.Age--;
            }
            return result;
        }
    }
}
