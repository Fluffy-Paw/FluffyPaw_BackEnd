using AutoMapper;
using FluffyPaw_Application.DTO.Request.AuthRequest;
using FluffyPaw_Application.DTO.Request.PetOwnerRequest;
using FluffyPaw_Application.DTO.Response.PetOwnerResponse;
using FluffyPaw_Application.Services;
using FluffyPaw_Domain.CustomException;
using FluffyPaw_Domain.Entities;
using FluffyPaw_Domain.Enums;
using FluffyPaw_Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Application.ServiceImplements
{
    public class PetOwnerService : IPetOwnerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAuthentication _authentication;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHashing _hashing;
        private readonly IFirebaseConfiguration _firebaseConfiguration;

        public PetOwnerService(IUnitOfWork unitOfWork, IMapper mapper, IAuthentication authentication, IHttpContextAccessor httpContextAccessor, IHashing hashing, IFirebaseConfiguration firebaseConfiguration)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _authentication = authentication;
            _httpContextAccessor = httpContextAccessor;
            _hashing = hashing;
            _firebaseConfiguration = firebaseConfiguration;
        }

        public async Task<PetOwnerResponse> GetPetOwnerDetail()
        {
            var accountId = _authentication.GetUserIdFromHttpContext(_httpContextAccessor.HttpContext);
            var po = _unitOfWork.PetOwnerRepository.Get(u => u.AccountId == accountId && u.Reputation != AccountReputation.Ban.ToString(), includeProperties: "Account").FirstOrDefault();
            if(po == null)
            {
                throw new CustomException.ForbbidenException("Bạn đã bị cấm, liên hệ admin để biết thêm thông tin.");
            }

            var result = _mapper.Map<PetOwnerResponse>(po);
            result.Dob = po.Dob.ToString("dd-MM-yyyy");
            result.Email = po.Account.Email;
            result.Avatar = po.Account.Avatar;
            return result;
        }

        public async Task<PetOwnerResponse> UpdatePetOwnerAccount(PetOwnerRequest petOwnerRequest)
        {
            var accountId = _authentication.GetUserIdFromHttpContext(_httpContextAccessor.HttpContext);
            var exitstingPo = _unitOfWork.PetOwnerRepository.Get(u => u.AccountId == accountId, includeProperties : "Account").FirstOrDefault();
            if (exitstingPo == null)
            {
                throw new CustomException.DataNotFoundException("Bạn không phải Pet Owner.");
            }

            exitstingPo.Account.Email = petOwnerRequest.Email;
            if(petOwnerRequest.Avatar != null) exitstingPo.Account.Avatar = await _firebaseConfiguration.UploadImage(petOwnerRequest.Avatar);
            
            var po = _mapper.Map(petOwnerRequest, exitstingPo);
            _unitOfWork.Save();

            var result = _mapper.Map<PetOwnerResponse>(po);
            result.Dob = exitstingPo.Dob.ToString("dd-MM-yyyy");

            return result;
        }
    }
}
