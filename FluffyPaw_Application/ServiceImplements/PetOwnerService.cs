using AutoMapper;
using FluffyPaw_Application.DTO.Request.PetOwnerRequest;
using FluffyPaw_Application.DTO.Response.PetOwnerResponse;
using FluffyPaw_Application.Services;
using FluffyPaw_Domain.CustomException;
using FluffyPaw_Domain.Entities;
using FluffyPaw_Domain.Interfaces;
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

        public PetOwnerService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PetOwner> GetPetOwnerDetail(long accountId)
        {
            var po = _unitOfWork.PetOwnerRepository.GetByID(accountId);
            po.Account = _unitOfWork.AccountRepository.GetByID(po.AccountId);
            if(po == null)
            {
                throw new CustomException.DataNotFoundException("Không tìm thấy user.");
            }
            return po;
        }

        public async Task<PetOwner> UpdatePetOwnerAccount(PetOwnerRequest petOwnerRequest)
        {
            var po = _unitOfWork.PetOwnerRepository.GetByID(petOwnerRequest.Id);
            po.Account = _unitOfWork.AccountRepository.GetByID(po.AccountId);
            if (po == null)
            {
                throw new CustomException.DataNotFoundException("Không tìm thấy user.");
            }

            var result = _mapper.Map(petOwnerRequest, po);
            _unitOfWork.Save();

            return result;
        }
    }
}
