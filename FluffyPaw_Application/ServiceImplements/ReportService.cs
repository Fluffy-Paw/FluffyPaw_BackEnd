using AutoMapper;
using FluffyPaw_Application.DTO.Request.ReportRequest;
using FluffyPaw_Application.DTO.Response.ReportResponse;
using FluffyPaw_Application.Services;
using FluffyPaw_Domain.CustomException;
using FluffyPaw_Domain.Entities;
using FluffyPaw_Domain.Interfaces;
using FluffyPaw_Domain.Utils;
using FluffyPaw_Repository.Enum;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace FluffyPaw_Application.ServiceImplements
{
    public class ReportService : IReportService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAuthentication _authentication;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ReportService(IUnitOfWork unitOfWork, IMapper mapper, IAuthentication authentication,
                        IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _authentication = authentication;
        }

        public async Task<List<ReportResponse>> GetAllReport()
        {
            var reports = _unitOfWork.ReportRepository.Get(orderBy: ob => ob.OrderByDescending(o => o.CreateDate),
                                            includeProperties: "TargetAccount,SenderAccount,ReportCategory");
            if (!reports.Any())
            {
                throw new CustomException.DataNotFoundException("Không tìm thấy báo cáo nào.");
            }

            var reportResponses = new List<ReportResponse>();

            foreach (var report in reports)
            {
                var reportResponse = _mapper.Map<ReportResponse>(report);
                var senderAccount = _unitOfWork.AccountRepository.Get(a => a.Id == report.SenderId).FirstOrDefault();
                if (senderAccount.RoleName == RoleName.Staff.ToString())
                {
                    var store = _unitOfWork.StoreRepository.Get(s => s.AccountId == report.SenderId).FirstOrDefault();
                    var po = _unitOfWork.PetOwnerRepository.Get(s => s.AccountId == report.TargetId).FirstOrDefault();
                    reportResponse.SenderName = store.Name;
                    reportResponse.TargetName = po.FullName;
                    reportResponses.Add(reportResponse);
                }
                else if (senderAccount.RoleName == RoleName.PetOwner.ToString())
                {
                    var store = _unitOfWork.StoreRepository.Get(s => s.AccountId == report.TargetId).FirstOrDefault();
                    var po = _unitOfWork.PetOwnerRepository.Get(s => s.AccountId == report.SenderId).FirstOrDefault();
                    reportResponse.SenderName = po.FullName;
                    reportResponse.TargetName = store.Name;
                    reportResponses.Add(reportResponse);
                }
            }

            return reportResponses;
        }

        public async Task<List<ReportResponse>> GetAllReportByStoreId(long id)
        {
            var store = _unitOfWork.StoreRepository.GetByID(id);
            if (store == null)
            {
                throw new CustomException.DataNotFoundException("Không tìm thấy cửa hàng.");
            }

            var account = _unitOfWork.AccountRepository.Get(a => a.Id == store.AccountId).FirstOrDefault();

            var reports = _unitOfWork.ReportRepository.Get(rps => rps.TargetId == account.Id,
                                                orderBy: ob => ob.OrderByDescending(o => o.CreateDate),
                                                includeProperties: "TargetAccount,SenderAccount,ReportCategory");
            if (!reports.Any())
            {
                throw new CustomException.DataNotFoundException("Không tìm thấy báo cáo này.");
            }

            var reportResponses = new List<ReportResponse>();

            foreach (var report in reports)
            {
                var reportResponse = _mapper.Map<ReportResponse>(report);
                var po = _unitOfWork.PetOwnerRepository.Get(po => po.AccountId == report.SenderId).FirstOrDefault();
                reportResponse.SenderName = po.FullName;
                reportResponse.TargetName = store.Name;
                reportResponses.Add(reportResponse);
            }

            return reportResponses;
        }

        public async Task<List<ReportResponse>> GetAllReportByPOId(long id)
        {
            var po = _unitOfWork.PetOwnerRepository.GetByID(id);
            if (po == null)
            {
                throw new CustomException.DataNotFoundException("Không tìm thấy chủ thú cưng.");
            }

            var account = _unitOfWork.AccountRepository.Get(a => a.Id == po.AccountId).FirstOrDefault();

            var reports = _unitOfWork.ReportRepository.Get(rps => rps.TargetId == account.Id,
                                                orderBy: ob => ob.OrderByDescending(o => o.CreateDate),
                                                includeProperties: "TargetAccount,SenderAccount,ReportCategory");
            if (!reports.Any())
            {
                throw new CustomException.DataNotFoundException("Không tìm thấy báo cáo này.");
            }

            var reportResponses = new List<ReportResponse>();

            foreach (var report in reports)
            {
                var reportResponse = _mapper.Map<ReportResponse>(report);
                var store = _unitOfWork.StoreRepository.Get(po => po.AccountId == report.SenderId).FirstOrDefault();
                reportResponse.SenderName = store.Name;
                reportResponse.TargetName = po.FullName;
                reportResponses.Add(reportResponse);
            }

            return reportResponses;
        }

        public async Task<List<ReportResponse>> GetAllTargetReportBySelf()
        {
            var userId = _authentication.GetUserIdFromHttpContext(_httpContextAccessor.HttpContext);
            var account = _unitOfWork.AccountRepository.GetByID(userId);
            if (account == null)
            {
                throw new CustomException.DataNotFoundException("Không tìm thấy tài khoản.");
            }

            var reports = _unitOfWork.ReportRepository.Get(rps => rps.TargetId == account.Id,
                                                orderBy: ob => ob.OrderByDescending(o => o.CreateDate),
                                                includeProperties: "TargetAccount,SenderAccount,ReportCategory");

            if (!reports.Any())
            {
                throw new CustomException.DataNotFoundException("Không tìm thấy báo cáo này.");
            }

            var reportResponses = new List<ReportResponse>();

            if (account.RoleName == RoleName.Staff.ToString())
            {
                foreach (var report in reports)
                {
                    var reportResponse = _mapper.Map<ReportResponse>(report);
                    var po = _unitOfWork.PetOwnerRepository.Get(s => s.AccountId == report.SenderId).FirstOrDefault();
                    if (po != null)
                    {
                        reportResponse.SenderName = po.FullName;
                    }

                    var store = _unitOfWork.StoreRepository.Get(po => po.AccountId == account.Id).FirstOrDefault();
                    if (store != null)
                    {
                        reportResponse.TargetName = store.Name;
                    }
                    reportResponses.Add(reportResponse);
                }
            }
            else if (account.RoleName == RoleName.PetOwner.ToString())
            {
                foreach (var report in reports)
                {
                    var reportResponse = _mapper.Map<ReportResponse>(report);
                    var store = _unitOfWork.StoreRepository.Get(s => s.AccountId == report.SenderId).FirstOrDefault();
                    if (store != null)
                    {
                        reportResponse.SenderName = store.Name;
                    }

                    var po = _unitOfWork.PetOwnerRepository.Get(po => po.AccountId == account.Id).FirstOrDefault();
                    if (po != null)
                    {
                        reportResponse.TargetName = po.FullName;
                    }

                    reportResponses.Add(reportResponse);
                }
            }

            return reportResponses;
        }

        public async Task<List<ReportCategoryResponse>> GetAllReportCategoryName()
        {
            var userId = _authentication.GetUserIdFromHttpContext(_httpContextAccessor.HttpContext);
            var account = _unitOfWork.AccountRepository.GetByID(userId);
            if (account == null)
            {
                throw new CustomException.DataNotFoundException("Không tìm thấy tài khoản.");
            }

            var reportCategories = _unitOfWork.ReportCategoryRepository.Get(rpcs => rpcs.Type == account.RoleName
                                                    || rpcs.Type == "General");
            if (!reportCategories.Any())
            {
                throw new CustomException.DataNotFoundException("Không tìm thấy danh sách báo cáo.");
            }

            var reportCategoryResponses = _mapper.Map<List<ReportCategoryResponse>>(reportCategories);
            return reportCategoryResponses;
        }


        public async Task<ReportResponse> CreateReport(CreateReportRequest createReportRequest)
        {
            var userId = _authentication.GetUserIdFromHttpContext(_httpContextAccessor.HttpContext);
            var senderAccount = _unitOfWork.AccountRepository.GetByID(userId);
            if (senderAccount == null)
            {
                throw new CustomException.DataNotFoundException("Không tìm thấy tài khoản.");
            }

            var requiredTargetRole = senderAccount.RoleName == RoleName.Staff.ToString()
                             ? RoleName.PetOwner.ToString()
                             : RoleName.Staff.ToString();

            var targetAccount = _unitOfWork.AccountRepository.Get(a => a.Id == createReportRequest.TargetId
                                                && a.RoleName == requiredTargetRole).FirstOrDefault();
            if (targetAccount == null)
            {
                throw new CustomException.DataNotFoundException("Không tìm thấy tài khoản để báo cáo.");
            }

            var reportCategories = _unitOfWork.ReportCategoryRepository.Get(rpcs => rpcs.Type == senderAccount.RoleName
                                                    || rpcs.Type == "General");
            if (!reportCategories.Any(p => p.Id == createReportRequest.ReportCategoryId))
            {
                throw new CustomException.InvalidDataException("Báo cáo này không thuộc quyền sử dụng của bạn.");
            }

            var newReport = new Report
            {
                SenderId = senderAccount.Id,
                TargetId = targetAccount.Id,
                ReportCategoryId = createReportRequest.ReportCategoryId,
                CreateDate = CoreHelper.SystemTimeNow,
                Description = createReportRequest.Description,
                Status = true
            };
            _unitOfWork.ReportRepository.Insert(newReport);
            _unitOfWork.Save();

            string senderName, targetName;

            if (senderAccount.RoleName == RoleName.Staff.ToString())
            {
                var store = _unitOfWork.StoreRepository.Get(s => s.AccountId == senderAccount.Id).FirstOrDefault();
                var petOwner = _unitOfWork.PetOwnerRepository.Get(po => po.AccountId == targetAccount.Id).FirstOrDefault();

                senderName = store?.Name ?? "Unknown Store";
                targetName = petOwner?.FullName ?? "Unknown PetOwner";
            }
            else
            {
                var petOwner = _unitOfWork.PetOwnerRepository.Get(po => po.AccountId == senderAccount.Id).FirstOrDefault();
                var store = _unitOfWork.StoreRepository.Get(s => s.AccountId == targetAccount.Id).FirstOrDefault();

                senderName = petOwner?.FullName ?? "Unknown PetOwner";
                targetName = store?.Name ?? "Unknown Store";
            }

            // Mapping ReportResponse và set thêm SenderName, TargetName
            var reportResponse = _mapper.Map<ReportResponse>(newReport);
            reportResponse.SenderName = senderName;
            reportResponse.TargetName = targetName;
            return reportResponse;
        }

        public async Task<bool> DeleteReport(long id)
        {
            var userId = _authentication.GetUserIdFromHttpContext(_httpContextAccessor.HttpContext);
            var senderAccount = _unitOfWork.AccountRepository.GetByID(userId);
            if (senderAccount == null)
            {
                throw new CustomException.DataNotFoundException("Không tìm thấy tài khoản.");
            }

            var report = _unitOfWork.ReportRepository.Get(rp => rp.Id == id && rp.SenderId == senderAccount.Id).FirstOrDefault();
            if (report == null)
            {
                throw new CustomException.DataNotFoundException("Không tìm thấy báo cáo này.");
            }

            _unitOfWork.ReportRepository.Delete(report);
            _unitOfWork.Save();

            return true;
        }
    }
}
