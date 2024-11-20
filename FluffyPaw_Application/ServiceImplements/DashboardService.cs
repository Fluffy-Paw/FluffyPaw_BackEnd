using FluffyPaw_Application.DTO.Response.DasboardResponse;
using FluffyPaw_Application.Services;
using FluffyPaw_Domain.Interfaces;
using FluffyPaw_Repository.Enum;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Application.ServiceImplements
{
    public class DashboardService : IDashboardService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthentication _authentication;
        private readonly IHttpContextAccessor _contextAccessor;

        public DashboardService(IUnitOfWork unitOfWork, IAuthentication authentication, IHttpContextAccessor contextAccessor)
        {
            _unitOfWork = unitOfWork;
            _authentication = authentication;
            _contextAccessor = contextAccessor;
        }

        public async Task<AdminDashboardResponse> GetAllStaticsAdmin()
        {
            var accounts = _unitOfWork.AccountRepository.GetAll().ToList();

            int numPO = accounts.Count(po => po.RoleName.Equals(RoleName.PetOwner.ToString()));
            int numSM = accounts.Count(sm => sm.RoleName.Equals(RoleName.StoreManager.ToString()));
            int numStore = accounts.Count(s => s.RoleName.Equals(RoleName.Staff.ToString()));

            var response = new AdminDashboardResponse
            {
                TotalPOs = numPO,
                TotalSMs = numSM,
                TotalStore = numStore,
                TotalUser = accounts.Count
            };

            return response;
        }

        public async Task<SMDashboardResponse> GetAllStaticsSM()
        {
            var userId = _authentication.GetUserIdFromHttpContext(_contextAccessor.HttpContext);
            var brand = _unitOfWork.BrandRepository.Get(u => u.AccountId.Equals(userId)).FirstOrDefault();
            var stores = _unitOfWork.StoreRepository.Get(b => b.BrandId.Equals(brand.Id)).ToList();
            var storeIds = stores.Select(s => s.Id).ToList();
            var services = _unitOfWork.StoreServiceRepository.Get(s => storeIds.Contains(s.StoreId)).ToList();
            var serviceIds = services.Select(s => s.Id).ToList();
            var booking = _unitOfWork.BookingRepository.Get(s => serviceIds.Contains(s.StoreServiceId)).ToList();

            int numOfAll = booking.Count;
            int NumOfAccepted = booking.Count(a => a.Status.Equals(BookingStatus.Accepted.ToString()));
            int NumOfCanceled = booking.Count(c => c.Status.Equals(BookingStatus.Canceled.ToString()));
            int NumOfPending = booking.Count(p => p.Status.Equals(BookingStatus.Pending.ToString()));

            var response = new SMDashboardResponse
            {
                NumOfAll = numOfAll,
                NumOfAccepted = NumOfAccepted,
                NumOfCanceled = NumOfCanceled,
                NumOfPending = NumOfPending
            };

            return response;
        }

        public async Task<AdminDashboardResponse> GetMonthStaticsAdmin(int month)
        {
            var accounts = _unitOfWork.AccountRepository.Get(m => m.CreateDate.Month == month).ToList();

            int numPO = accounts.Count(po => po.RoleName.Equals(RoleName.PetOwner.ToString()));
            int numSM = accounts.Count(sm => sm.RoleName.Equals(RoleName.StoreManager.ToString()));
            int numStore = accounts.Count(s => s.RoleName.Equals(RoleName.Staff.ToString()));

            var response = new AdminDashboardResponse
            {
                TotalPOs = numPO,
                TotalSMs = numSM,
                TotalStore = numStore,
                TotalUser = accounts.Count
            };

            return response;
        }

        public async Task<SMDashboardResponse> GetMonthStaticsSM(int month)
        {
            var userId = _authentication.GetUserIdFromHttpContext(_contextAccessor.HttpContext);
            var brand = _unitOfWork.BrandRepository.Get(u => u.AccountId.Equals(userId)).FirstOrDefault();
            var stores = _unitOfWork.StoreRepository.Get(b => b.BrandId.Equals(brand.Id)).ToList();
            var storeIds = stores.Select(s => s.Id).ToList();
            var services = _unitOfWork.StoreServiceRepository.Get(s => storeIds.Contains(s.StoreId)).ToList();
            var serviceIds = services.Select(s => s.Id).ToList();
            var booking = _unitOfWork.BookingRepository.Get(s => serviceIds.Contains(s.StoreServiceId) && s.CreateDate.Month == month).ToList();

            int numOfAll = booking.Count;
            int NumOfAccepted = booking.Count(a => a.Status.Equals(BookingStatus.Accepted.ToString()));
            int NumOfCanceled = booking.Count(c => c.Status.Equals(BookingStatus.Canceled.ToString()));
            int NumOfPending = booking.Count(p => p.Status.Equals(BookingStatus.Pending.ToString()));

            var response = new SMDashboardResponse
            {
                NumOfAll = numOfAll,
                NumOfAccepted = NumOfAccepted,
                NumOfCanceled = NumOfCanceled,
                NumOfPending = NumOfPending
            };

            return response;
        }
    }
}
