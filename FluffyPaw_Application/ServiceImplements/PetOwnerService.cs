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

        public async Task<PetOwnerResponse> GetPetOwnerDetail(long accountId)
        {
            var po = _unitOfWork.PetOwnerRepository.Get(u => u.AccountId == accountId, includeProperties: "Account");
            if(po == null)
            {
                throw new CustomException.DataNotFoundException("Không tìm thấy user.");
            }

            return _mapper.Map<PetOwnerResponse>(po);
        }

        public async Task<bool> UpdatePetOwnerAccount(PetOwnerRequest petOwnerRequest)
        {
            throw new NotImplementedException();
        }
    }
}
