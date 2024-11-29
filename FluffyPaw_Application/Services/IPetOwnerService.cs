using FluffyPaw_Application.DTO.Request.BookingRequest;
using FluffyPaw_Application.DTO.Request.PetOwnerRequest;
using FluffyPaw_Application.DTO.Response.BookingResponse;
using FluffyPaw_Application.DTO.Response.BrandResponse;
using FluffyPaw_Application.DTO.Response.PetOwnerResponse;
using FluffyPaw_Application.DTO.Response.ServiceResponse;
using FluffyPaw_Application.DTO.Response.StaffResponse;
using FluffyPaw_Application.DTO.Response.StoreManagerResponse;
using FluffyPaw_Application.DTO.Response.StoreServiceResponse;
using FluffyPaw_Domain.Entities;

namespace FluffyPaw_Application.Services
{
    public interface IPetOwnerService
    {
        Task<PetOwnerResponse> UpdatePetOwnerAccount(PetOwnerRequest petOwnerRequest);

        Task<PetOwnerResponse> GetPetOwnerDetail();

        Task<BrResponse> GetBrandById(long id);

        Task<List<StoreResponse>> GetAllStore();

        Task<List<StoreResponse>> GetAllStoreByBrandId(long id);

        Task<List<StoreResponse>> GetAllStoreByServiceTypeId(long id);

        Task<StoreSerResponse> GetStoreServiceById(long id);

        Task<List<StoreSerResponse>> SuggestionSameTimeAndBrand(long id);

        Task<List<StoreSerResponse>> SuggestionSameTime(long id);

        Task<StResponse> GetStoreById(long id);

        Task<List<StoreSerResponse>> GetAllStoreServiceByServiceIdStoreId(long serviceId, long storeId);

        Task<List<SerResponse>> GetAllServiceByServiceTypeIdDateTime(long serviceTypeId, DateTimeOffset? startTime, DateTimeOffset? endTime);

        Task<List<StResponse>> GetAllStoreByServiceIdDateTime(long serviceId, DateTimeOffset? startTime, DateTimeOffset? endTime);

        Task<List<StoreSerResponse>> GetAllStoreServiceByServiceId(long id);

        Task<List<BookingResponse>> GetAllBooking();

        Task<List<BookingResponse>> GetAllBookingByPetId(long id, string? bookingStatus);

        Task<List<BookingResponse>> CreateBooking(CreateBookingRequest createBookingRequest);

        Task<BookingResponse> CreateBookingTimeSelection(TimeSelectionRequest timeSelectionRequest);

        Task<bool> CancelBooking(long id);

        Task<List<BillingRecordResponse>> GetAllBillingRecord();

        Task<List<TrackingResponse>> GetAllTrackingByBookingId(long id);

        Task<TrackingResponse> GetTrackingById(long id);

        Task<List<StoreServicePOResponse>> RecommendServicePO();

        Task<List<StoreServicePOResponse>> RecommendServiceGuest();

        Task<List<StoreServicePOResponse>> Top6StoreServices();

        Task<List<Store>> SearchStore(string character);
        
        Task<List<StoreServicePOResponse>> SearchStoreService(string character);

        Task<List<SearchBrandResponse>> SearchBrand(string character);
    }
}
