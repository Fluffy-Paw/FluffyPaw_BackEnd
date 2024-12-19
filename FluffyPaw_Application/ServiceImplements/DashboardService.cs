using FluffyPaw_Application.DTO.Request.DashboardRequest;
using FluffyPaw_Application.DTO.Response.DasboardResponse;
using FluffyPaw_Application.Services;
using FluffyPaw_Domain.Entities;
using FluffyPaw_Domain.Enums;
using FluffyPaw_Domain.Interfaces;
using FluffyPaw_Domain.Utils;
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
            var transactions = _unitOfWork.TransactionRepository.GetAll().ToList();

            int numPO = accounts.Count(po => po.RoleName.Equals(RoleName.PetOwner.ToString()));
            int numSM = accounts.Count(sm => sm.RoleName.Equals(RoleName.StoreManager.ToString()));
            int numStore = accounts.Count(s => s.RoleName.Equals(RoleName.Staff.ToString()));

            List<double> withdrawRevenues = new List<double>();
            List<double> depositRevenues = new List<double>();

            for (int i = 1; i <= 12; i++)
            {
                var transactionsByMonth = transactions.FindAll(b => b.CreateTime.Month == i && b.Type == "Rút tiền");
                double revenue = transactionsByMonth.Sum(r => r.Amount);
                withdrawRevenues.Add(revenue);
            }

            for (int i = 1; i <= 12; i++)
            {
                var transactionsByMonth = transactions.FindAll(b => b.CreateTime.Month == i && b.Type == "Nạp tiền");
                double revenue = transactionsByMonth.Sum(r => r.Amount);
                depositRevenues.Add(revenue);
            }


            var response = new AdminDashboardResponse
            {
                TotalPOs = numPO,
                TotalSMs = numSM,
                TotalStore = numStore,
                TotalUser = accounts.Count,
                DepositRevenues = depositRevenues,
                WithdrawRevenues = withdrawRevenues
            };

            return response;
        }

        public async Task<SMDashboardResponse> GetAllStaticsSM()
        {
            var userId = _authentication.GetUserIdFromHttpContext(_contextAccessor.HttpContext);
            var brand = _unitOfWork.BrandRepository.Get(u => u.AccountId.Equals(userId)).FirstOrDefault();
            var stores = _unitOfWork.StoreRepository.Get(b => b.BrandId.Equals(brand.Id)).ToList();
            var storeIds = stores.Select(s => s.Id).ToList();
            var storeServices = _unitOfWork.StoreServiceRepository.Get(s => storeIds.Contains(s.StoreId), includeProperties: "Store,Service").ToList();
            var storeServiceIds = storeServices.Select(s => s.Id).ToList();
            var storeServiceServiceIds = storeServices.Select(s => s.ServiceId).ToList();
            var services = _unitOfWork.ServiceRepository.Get(s => storeServiceServiceIds.Contains(s.Id), includeProperties: "ServiceType").ToList();
            var bookings = _unitOfWork.BookingRepository.Get(b => storeServiceIds.Contains(b.StoreServiceId) && b.CheckOut == true && b.StartTime.Year == CoreHelper.SystemTimeNow.Year).ToList();

            int numOfReports = 0;
            foreach (var store in stores)
            {
                var report = _unitOfWork.ReportRepository.Get(rp => rp.TargetId.Equals(store.AccountId)).ToList();
                numOfReports += report.Count();
            }

            List<(string,List<double>)> AllServiceRevenues = new List<(string, List<double>)>();
            List<double> revenues = new List<double>();
            for (int i = 1; i <= 12; i++)
            {
                var bookingsByMonth = bookings.FindAll(b => b.CreateDate.Month == i);
                double revenue = bookingsByMonth.Sum(r => r.Cost);
                revenues.Add(revenue);
            }

            List<StoreServiceResponse> topServices = new List<StoreServiceResponse>();
            //var listService = services.OrderByDescending(s => s.BookingCount).Take(3).ToList();
            //foreach (var service in listService)
            //{
            //    var ss = new StoreServiceResponse
            //    {
            //        ServiceName = service.Name,
            //        NumberOfBooking = service.BookingCount
            //    };
            //    topServices.Add(ss);
            //}

            var storeServiceDict = storeServices.ToDictionary(ss => ss.Id);
            var countTopServices = bookings.GroupBy(b => b.StoreServiceId).Select(g => new { serviceId = g.Key, count = g.Count() }).OrderByDescending(ob => ob.count).Take(3).ToList();
            int id = 1;
            foreach (var item in countTopServices)
            {
                if (storeServiceDict.TryGetValue(item.serviceId, out var storeService))
                {
                    var ss = new StoreServiceResponse
                    {
                        Id = id,
                        StoreName = storeService.Store.Name,
                        ServiceName = storeService.Service.Name,
                        NumberOfBooking = item.count
                    };
                    id++;
                    topServices.Add(ss);
                }
            }

            var COD = bookings.Where(c => c.PaymentMethod.Equals(BookingPaymentMethod.COD.ToString())).ToList();
            var FluffyPay = bookings.Where(f => f.PaymentMethod.Equals(BookingPaymentMethod.FluffyPay.ToString())).ToList();

            var response = new SMDashboardResponse
            {
                NumOfAll = bookings.Count(),
                NumOfAccepted = bookings.Count(a => a.Status.Equals(BookingStatus.Accepted.ToString())),
                NumOfCanceled = bookings.Count(c => c.Status.Equals(BookingStatus.Canceled.ToString())),
                NumOfPending = bookings.Count(p => p.Status.Equals(BookingStatus.Pending.ToString())),
                NumOfStores = stores.Count(),
                NumOfReports = numOfReports,
                Revenues = revenues,
                TopServices = topServices,
                ServicesNames = services.Select(n => n.Name).ToList(),
                NumOfCOD = COD.Count(),
                NumOfFluffyPay = FluffyPay.Count(),
                AmountCOD = COD.Sum(c => c.Cost),
                AmountFluffyPay = FluffyPay.Sum(c => c.Cost)
            };

            return response;
        }

        public async Task<SMDashboardResponse> GetAllStaticsStaff()
        {
            var userId = _authentication.GetUserIdFromHttpContext(_contextAccessor.HttpContext);
            var store = _unitOfWork.StoreRepository.Get(b => b.Account.Id.Equals(userId)).FirstOrDefault();
            var storeServices = _unitOfWork.StoreServiceRepository.Get(s => s.StoreId == store.Id, includeProperties: "Store,Service").ToList();
            var storeServiceIds = storeServices.Select(s => s.Id).ToList();
            var storeServiceServiceIds = storeServices.Select(s => s.ServiceId).ToList();
            var services = _unitOfWork.ServiceRepository.Get(s => storeServiceServiceIds.Contains(s.Id)).ToList();
            var bookings = _unitOfWork.BookingRepository.Get(b => storeServiceIds.Contains(b.StoreServiceId), includeProperties: "StoreService").ToList();
            var report = _unitOfWork.ReportRepository.Get(rp => rp.TargetId.Equals(store.AccountId)).ToList();

            List<double> revenues = new List<double>();
            for (int i = 1; i <= 12; i++)
            {
                var bookingsByMonth = bookings.FindAll(b => b.CreateDate.Month == i);
                double revenue = bookingsByMonth.Sum(r => r.Cost);
                revenues.Add(revenue);
            }

            List<StoreServiceResponse> topServices = new List<StoreServiceResponse>();
            //var listService = services.OrderByDescending(s => s.BookingCount).Take(3).ToList();
            //foreach (var service in listService)
            //{
            //    var ss = new StoreServiceResponse
            //    {
            //        ServiceName = service.Name,
            //        NumberOfBooking = service.BookingCount
            //    };
            //    topServices.Add(ss);
            //}

            var serviceDict = services.ToDictionary(ss => ss.Id);
            var countTopServices = bookings.GroupBy(b => b.StoreService.ServiceId).Select(g => new { serviceId = g.Key, count = g.Count() }).OrderByDescending(ob => ob.count).Take(3).ToList();
            int id = 1;
            foreach (var item in countTopServices)
            {
                if (serviceDict.TryGetValue(item.serviceId, out var service))
                {
                    var ss = new StoreServiceResponse
                    {
                        Id = id,
                        StoreName = store.Name,
                        ServiceName = service.Name,
                        NumberOfBooking = item.count
                    };
                    id++;
                    topServices.Add(ss);
                }
            }

            var COD = bookings.Where(c => c.PaymentMethod.Equals(BookingPaymentMethod.COD.ToString())).ToList();
            var FluffyPay = bookings.Where(f => f.PaymentMethod.Equals(BookingPaymentMethod.FluffyPay.ToString())).ToList();

            var response = new SMDashboardResponse
            {
                NumOfAll = bookings.Count(),
                NumOfAccepted = bookings.Count(a => a.Status.Equals(BookingStatus.Accepted.ToString())),
                NumOfCanceled = bookings.Count(c => c.Status.Equals(BookingStatus.Canceled.ToString())),
                NumOfPending = bookings.Count(p => p.Status.Equals(BookingStatus.Pending.ToString())),
                NumOfStores = 1,
                NumOfReports = report.Count,
                Revenues = revenues,
                TopServices = topServices,
                ServicesNames = services.Select(n => n.Name).ToList(),
                NumOfCOD = COD.Count(),
                NumOfFluffyPay = FluffyPay.Count(),
                AmountCOD = COD.Sum(c => c.Cost),
                AmountFluffyPay = FluffyPay.Sum(c => c.Cost)
            };

            return response;
        }

        public async Task<double> GetMonthlyRevenueStaff(MonthlyRevenueRequest monthlyRevenueRequest)
        {
            var userId = _authentication.GetUserIdFromHttpContext(_contextAccessor.HttpContext);
            var store = _unitOfWork.StoreRepository.Get(b => b.AccountId.Equals(userId)).FirstOrDefault();
            var booking = _unitOfWork.BookingRepository.Get(b => b.StoreService.StoreId.Equals(store.Id) && b.StoreService.Service.Name.Equals(monthlyRevenueRequest.ServiceName) && b.CreateDate.Month.Equals(monthlyRevenueRequest.Month) && b.CreateDate.Year.Equals(CoreHelper.SystemTimeNow.Year), includeProperties: "StoreService,StoreService.Service").ToList();
            return booking.Sum(b => b.Cost);
        }

        public async Task<double> GetMonthlyRevenueSM(MonthlyRevenueRequest monthlyRevenueRequest)
        {
            var userId = _authentication.GetUserIdFromHttpContext(_contextAccessor.HttpContext);
            var brand = _unitOfWork.BrandRepository.Get(b => b.AccountId.Equals(userId)).FirstOrDefault();
            var store = _unitOfWork.StoreRepository.Get(b => b.BrandId.Equals(brand.Id)).FirstOrDefault();
            var booking = _unitOfWork.BookingRepository.Get(b => b.StoreService.StoreId.Equals(store.Id) && b.StoreService.Service.Name.Equals(monthlyRevenueRequest.ServiceName) && b.CreateDate.Month.Equals(monthlyRevenueRequest.Month) && b.CreateDate.Year.Equals(CoreHelper.SystemTimeNow.Year), includeProperties: "StoreService,StoreService.Service").ToList();
            return booking.Sum(b => b.Cost);
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
