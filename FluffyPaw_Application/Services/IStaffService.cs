﻿using FluffyPaw_Application.DTO.Request.StoreManagerRequest;
using FluffyPaw_Application.DTO.Request.StoreServiceRequest;
using FluffyPaw_Application.DTO.Response.BookingResponse;
using FluffyPaw_Application.DTO.Response.ServiceResponse;
using FluffyPaw_Application.DTO.Response.StoreManagerResponse;
using FluffyPaw_Application.DTO.Response.StoreServiceResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Application.Services
{
    public interface IStaffService
    {
        Task<StoreResponse> GetStoreByStaff();
        Task<List<SerResponse>> GetAllServiceByBrandId(long id);
        Task<List<StoreServiceResponse>> CreateStoreService(CreateStoreServiceRequest createStoreServiceRequest);
        Task<StoreServiceResponse> UpdateStoreService(long id, UpdateStoreServiceRequest updateStoreServiceRequest);
        Task<bool> DeleteStoreService(long id);
        Task<List<StoreBookingResponse>> GetAllBookingByStore();
        Task<List<BookingResponse>> GetAllBookingByStoreServiceId(long id);
        Task<bool> AcceptBooking(long id);
        Task<bool> DeniedBooking(long id);
    }
}