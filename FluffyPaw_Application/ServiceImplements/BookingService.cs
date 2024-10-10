using AutoMapper;
using FluffyPaw_Application.DTO.Response.BookingResponse;
using FluffyPaw_Application.Services;
using FluffyPaw_Domain.CustomException;
using FluffyPaw_Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Application.ServiceImplements
{
    public class BookingService : IBookingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BookingService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<BookingResponse> GetBookingById(long id)
        {
            var existingBooking = _unitOfWork.BookingRepository.GetByID(id);
            if (existingBooking == null)
            {
                throw new CustomException.DataNotFoundException("Không tìm thấy đặt lịch này.");
            }

            var bookingResponse = _mapper.Map<BookingResponse>(existingBooking);
            return bookingResponse;
        }
    }
}
