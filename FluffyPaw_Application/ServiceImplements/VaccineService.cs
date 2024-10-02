using AutoMapper;
using FluffyPaw_Application.DTO.Request.VacineRequest;
using FluffyPaw_Application.DTO.Response.VaccineResponse;
using FluffyPaw_Application.Services;
using FluffyPaw_Domain.CustomException;
using FluffyPaw_Domain.Entities;
using FluffyPaw_Domain.Enums;
using FluffyPaw_Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Application.ServiceImplements
{
    public class VaccineService : IVaccineService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IFirebaseConfiguration _firebaseConfiguration;

        public VaccineService(IUnitOfWork unitOfWork, IMapper mapper, IFirebaseConfiguration firebaseConfiguration)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _firebaseConfiguration = firebaseConfiguration;
        }

        public async Task<bool> AddVacine(VaccineRequest vaccineRequest)
        {
            var pet = _unitOfWork.PetRepository.GetByID(vaccineRequest.PetId);
            if (pet == null)
            {
                throw new CustomException.DataNotFoundException("Không tìm thấy thú cưng.");
            }
            if (vaccineRequest.VaccineDate >  DateTime.UtcNow)
            {
                throw new CustomException.InvalidDataException("Ngày không hợp lệ.");
            }
            var vaccine = _mapper.Map<VaccineHistory>(vaccineRequest);
            if(vaccineRequest.Image != null) vaccine.Image = await _firebaseConfiguration.UploadImage(vaccineRequest.Image);
            vaccine.Status = VaccineStatus.Uncomplete.ToString();

            _unitOfWork.VaccineHistoryRepository.Insert(vaccine);
            _unitOfWork.Save();

            return true;
        }

        public Task<IEnumerable<ListVaccineResponse>> GetVaccineHistories(long petId)
        {
            throw new NotImplementedException();
        }

        public Task<VaccineHistory> GetVaccineHistory(long vaccineId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveVacine(long vaccineId)
        {
            throw new NotImplementedException();
        }

        public Task<VaccineHistory> UpdateVaccine(VaccineRequest vaccineRequest)
        {
            throw new NotImplementedException();
        }
    }
}
