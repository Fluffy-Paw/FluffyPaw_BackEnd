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

        public async Task<bool> AddVaccine(VaccineRequest vaccineRequest)
        {
            var pet = _unitOfWork.PetRepository.GetByID(vaccineRequest.PetId);
            if (pet == null)
            {
                throw new CustomException.DataNotFoundException("Không tìm thấy thú cưng.");
            }
            if (vaccineRequest.NextVaccineDate < vaccineRequest.VaccineDate || vaccineRequest.VaccineDate > DateTimeOffset.UtcNow)
            {
                throw new CustomException.InvalidDataException("Ngày của vaccine không hợp lệ");
            }
            var vaccine = _mapper.Map<VaccineHistory>(vaccineRequest);
            if(vaccineRequest.VaccineImage != null) vaccine.Image = await _firebaseConfiguration.UploadImage(vaccineRequest.VaccineImage);
            if(vaccine.NextVaccineDate == null) vaccine.Status = VaccineStatus.Complete.ToString();
            else vaccine.Status = VaccineStatus.Incomplete.ToString();

            _unitOfWork.VaccineHistoryRepository.Insert(vaccine);
            _unitOfWork.Save();

            return true;
        }

        public async Task<IEnumerable<ListVaccineResponse>> GetVaccineHistories(long petId)
        {
            var vaccineList = _unitOfWork.VaccineHistoryRepository.Get(v => v.PetId == petId && v.Status != VaccineStatus.Deleted.ToString(), orderBy: ob => ob.OrderByDescending(v => v.VaccineDate));
            if(!vaccineList.Any())
            {
                throw new CustomException.DataNotFoundException("Bạn chưa nhập thông tin vaccine.");
            }

            return _mapper.Map<IEnumerable<ListVaccineResponse>>(vaccineList);
        }

        public async Task<VaccineHistory> GetVaccineHistory(long vaccineId)
        {
            return _unitOfWork.VaccineHistoryRepository.GetByID(vaccineId);
        }

        public async Task<bool> RemoveVacine(long vaccineId)
        {
            var vaccine = _unitOfWork.VaccineHistoryRepository.GetByID(vaccineId);
            if(vaccine == null)
            {
                throw new CustomException.DataNotFoundException("Không tìm thấy vaccine cần xóa.");
            }
            vaccine.Status = VaccineStatus.Deleted.ToString();
            _unitOfWork.Save();

            return true;
        }

        public async Task<VaccineHistory> CheckoutVaccine(long vaccineId)
        {
            var vaccine = _unitOfWork.VaccineHistoryRepository.GetByID(vaccineId);
            if (vaccine == null)
            {
                throw new CustomException.DataNotFoundException("Không tìm thấy vaccine.");
            }
            if (vaccine.NextVaccineDate > DateTimeOffset.UtcNow)
            {
                throw new CustomException.ForbbidenException("Chưa đến ngày chích vaccine");
            }

            vaccine.Status = VaccineStatus.Complete.ToString();
            _unitOfWork.Save();

            return vaccine;
        }

        public async Task<VaccineHistory> UpdateVaccineHistory(long vaccineId,VaccineRequest vaccineRequest)
        {
            var vaccine = _unitOfWork.VaccineHistoryRepository.GetByID(vaccineId);
            if (vaccine == null)
            {
                throw new CustomException.DataNotFoundException("Không tìm thấy vaccine cần cập nhật.");
            }
            if (vaccineRequest.NextVaccineDate < vaccineRequest.VaccineDate || vaccineRequest.VaccineDate > DateTimeOffset.UtcNow)
            {
                throw new CustomException.InvalidDataException("Ngày của vaccine không hợp lệ");
            }

            _mapper.Map(vaccineRequest, vaccine);

            if (vaccine.NextVaccineDate > DateTimeOffset.UtcNow) vaccine.Status = VaccineStatus.Incomplete.ToString();
            else vaccine.Status = VaccineStatus.Complete.ToString();
            _unitOfWork.Save();

            return vaccine;
        }
    }
}
