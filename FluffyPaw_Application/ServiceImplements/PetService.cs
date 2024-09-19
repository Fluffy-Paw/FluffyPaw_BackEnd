using AutoMapper;
using FluffyPaw_Application.DTO.Request.PetRequest;
using FluffyPaw_Application.DTO.Response.PetResponse;
using FluffyPaw_Application.Services;
using FluffyPaw_Domain.CustomException;
using FluffyPaw_Domain.Interfaces;
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
        private readonly IConfiguration _configuration;

        public PetService(IUnitOfWork unitOfWork, IMapper mapper, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _configuration = configuration;
        }

        public Task<bool> CreateNewPet(PetRequest petRequest)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeletePet(long petId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<PetResponse>> GetAllPetOfUser(long userId)
        {
            var existingPet = _unitOfWork.PetRepository.Get(p => p.PetOwnerId == userId);
            if(!existingPet.Any())
            {
                throw new CustomException.DataNotFoundException("Bạn chưa nhập thú cưng.");
            }
            return _mapper.Map<IEnumerable<PetResponse>>(existingPet);
        }

        public Task<bool> UpdatePet(PetRequest petRequest)
        {
            throw new NotImplementedException();
        }
    }
}
