using FluffyPaw_Application.DTO.Request.BookingRequest;
using FluffyPaw_Application.DTO.Request.PetOwnerRequest;
using FluffyPaw_Application.DTO.Response.BookingResponse;
using FluffyPaw_Application.DTO.Response.PetOwnerResponse;
using FluffyPaw_Application.DTO.Response.StaffResponse;
using FluffyPaw_Application.DTO.Response.StoreManagerResponse;
using FluffyPaw_Application.DTO.Response.StoreServiceResponse;

namespace FluffyPaw_Application.Services
{
    public interface IPetOwnerService
    {
        Task<PetOwnerResponse> UpdatePetOwnerAccount(PetOwnerRequest petOwnerRequest);

        Task<PetOwnerResponse> GetPetOwnerDetail();

        Task<List<StoreResponse>> GetAllStore();

        Task<List<StoreResponse>> GetAllStoreByBrandId(long id);

        Task<List<StoreResponse>> GetAllStoreByServiceTypeId(long id);

        Task<StoreSerResponse> GetStoreServiceById(long id);

        Task<List<StoreSerResponse>> SuggestionSameTimeAndBrand(long id);

        Task<List<StoreSerResponse>> SuggestionSameTime(long id);

        Task<List<StoreResponse>> GetStoreById(long id);

        Task<List<StoreSerResponse>> GetAllStoreServiceByServiceId(long id);

        Task<List<BookingResponse>> GetAllBooking();

        Task<List<BookingResponse>> GetAllBookingByPetId(long id, string? bookingStatus);

        Task<List<BookingResponse>> CreateBooking(CreateBookingRequest createBookingRequest);

        Task<BookingResponse> CreateBookingTimeSelection(TimeSelectionRequest timeSelectionRequest);

        Task<bool> CancelBooking(long id);

        Task<List<TrackingResponse>> GetAllTrackingByBookingId(long id);

        Task<TrackingResponse> GetTrackingById(long id);

        Task<List<StoreSerResponse>> RecommendServicePO();

        Task<List<StoreSerResponse>> RecommendServiceGuest();
    }
}
