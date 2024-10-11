using FluffyPaw_Application.DTO.Request.BookingRequest;
using FluffyPaw_Application.DTO.Request.PetOwnerRequest;
using FluffyPaw_Application.DTO.Request.StoreServiceRequest;
using FluffyPaw_Application.DTO.Response.BookingResponse;
using FluffyPaw_Application.DTO.Response.PetOwnerResponse;
using FluffyPaw_Application.DTO.Response.StoreServiceResponse;
using FluffyPaw_Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Application.Services
{
    public interface IPetOwnerService
    {
        Task<PetOwnerResponse> UpdatePetOwnerAccount(PetOwnerRequest petOwnerRequest);
        Task<PetOwnerResponse> GetPetOwnerDetail();
        Task<List<BookingResponse>> GetAllBookingByPetId(long id);
        Task<BookingResponse> CreateBooking(CreateBookingRequest createBookingRequest);
        Task<bool> CancelBooking(long id);

    }
}
