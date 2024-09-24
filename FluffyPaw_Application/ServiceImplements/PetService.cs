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

        public PetService(IUnitOfWork unitOfWork, IMapper mapper, IFirebaseConfiguration firebaseConfiguration)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _firebaseConfiguration = firebaseConfiguration;
        }

        public async Task<bool> CreateNewPet(PetRequest petRequest)
        {
            var existingPet = _unitOfWork.PetRepository.Get(po=>po.PetOwnerId == petRequest.PetOwnerId && po.Status == PetStatus.Available.ToString());
            if (existingPet.Count() >= 5)
            {
                throw new CustomException.InvalidDataException("Bạn chỉ được lưu tối đa 5 thú cưng.");
            }

            var pet = _mapper.Map<Pet>(petRequest);
            if(petRequest.Image != null)
            {
                pet.Image = await _firebaseConfiguration.UploadImage(petRequest.Image);
            }
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

        public async Task<IEnumerable<PetResponse>> GetAllPetOfUser(long userId)
        {
            var existingPet = _unitOfWork.PetRepository.Get(p => p.PetOwnerId == userId && p.Status == PetStatus.Available.ToString());
            if(!existingPet.Any())
            {
                throw new CustomException.DataNotFoundException("Bạn chưa nhập thông tin thú cưng.");
            }
            return _mapper.Map<IEnumerable<PetResponse>>(existingPet);
        }

        public async Task<PetResponse> UpdatePet(long petId, PetRequest petRequest)
        {
            var existingPet = _unitOfWork.PetRepository.GetByID(petId);
            if(existingPet == null)
            {
                throw new CustomException.DataNotFoundException("Không tìm thấy thú cưng.");
            }

            if (petRequest.Image != null)
            {
                existingPet.Image = await _firebaseConfiguration.UploadImage(petRequest.Image);
            }
            _mapper.Map(petRequest, existingPet);
            _unitOfWork.Save();

            return _mapper.Map<PetResponse>(existingPet);
        }
    }
}
