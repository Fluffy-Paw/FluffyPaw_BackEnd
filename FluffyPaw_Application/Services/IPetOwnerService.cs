using FluffyPaw_Application.DTO.Request.BookingRequest;
using FluffyPaw_Application.DTO.Request.PetOwnerRequest;
using FluffyPaw_Application.DTO.Response.BookingResponse;
using FluffyPaw_Application.DTO.Response.PetOwnerResponse;
using FluffyPaw_Application.DTO.Response.StoreManagerResponse;

namespace FluffyPaw_Application.Services
{
    public interface IPetOwnerService
    {
        Task<PetOwnerResponse> UpdatePetOwnerAccount(PetOwnerRequest petOwnerRequest);
        Task<PetOwnerResponse> GetPetOwnerDetail();
        Task<List<StoreResponse>> GetAllStore();
        Task<List<StoreResponse>> GetStoreById(long id);
        Task<List<BookingResponse>> GetAllBookingByPetId(long id, string? bookingStatus);
        Task<BookingResponse> CreateBooking(CreateBookingRequest createBookingRequest);
        Task<bool> CancelBooking(long id);

    }
}
