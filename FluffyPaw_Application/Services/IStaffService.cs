﻿using FluffyPaw_Application.DTO.Request.BookingRequest;
using FluffyPaw_Application.DTO.Request.StoreManagerRequest;
using FluffyPaw_Application.DTO.Request.StoreServiceRequest;
using FluffyPaw_Application.DTO.Request.TrackingRequest;
using FluffyPaw_Application.DTO.Response.BookingResponse;
using FluffyPaw_Application.DTO.Response.FilesResponse;
using FluffyPaw_Application.DTO.Response.ServiceResponse;
using FluffyPaw_Application.DTO.Response.StaffResponse;
using FluffyPaw_Application.DTO.Response.StoreManagerResponse;
using FluffyPaw_Application.DTO.Response.StoreServiceResponse;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Application.Services
{
    public interface IStaffService
    {
        Task<List<SerResponse>> GetAllServiceByBrandId(long id);
        Task<StoreResponse> GetStoreByStaff();
        Task<FileResponse> GetStoreImageById(long id);
        Task<FileResponse> UpdateStoreImage(long id, IFormFile file);
        Task<bool> DeleteImage(long id);
        Task<List<StoreSerResponse>> GetAllStoreServiceByServiceId(long id);
        Task<List<StoreSerResponse>> CreateScheduleStoreService(ScheduleStoreServiceRequest scheduleStoreServiceRequest);
        Task<List<StoreSerResponse>> CreateStoreService(CreateStoreServiceRequest createStoreServiceRequest);
        Task<bool> UpdateStoreService(long id, UpdateStoreServiceRequest updateStoreServiceRequest);
        Task<bool> DeleteStoreService(long id);
        Task<List<StoreBookingResponse>> GetAllBookingByStore(FilterBookingRequest filterBookingRequest);
        Task<List<BookingResponse>> GetAllBookingByStoreServiceId(long id);
        Task<bool> AcceptBooking(long id);
        Task<bool> DeniedBooking(long id);
        Task<(bool isSuccess, string notice)> CancelBooking(long id);
        Task<List<TrackingResponse>> GetAllTrackingByBookingId(long id);
        Task<TrackingResponse> GetTrackingById(long id);
        Task<TrackingResponse> CreateTracking(TrackingRequest trackingRequest);
        Task<TrackingResponse> UpdateTracking(long id, UpdateTrackingRequest updateTrackingRequest);
        Task<bool> DeleteTracking(long id);
    }
}
