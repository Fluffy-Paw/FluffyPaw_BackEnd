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
                                            includeProperties: "SenderAccount,TargetAccount");
            if (!reports.Any())
            {
                throw new CustomException.DataNotFoundException("Không tìm thấy báo cáo nào.");
            }

            var reportResponses = _mapper.Map<List<ReportResponse>>(reports);
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
                                                includeProperties: "TargetAccount,SenderAccount");
            if (!reports.Any())
            {
                throw new CustomException.DataNotFoundException("Không tìm thấy báo cáo này.");
            }

            var reportResponses = _mapper.Map<List<ReportResponse>>(reports);
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
                                                includeProperties: "TargetAccount,SenderAccount");
            if (!reports.Any())
            {
                throw new CustomException.DataNotFoundException("Không tìm thấy báo cáo này.");
            }

            var reportResponses = _mapper.Map<List<ReportResponse>>(reports);
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

            var targetAccount = _unitOfWork.AccountRepository.Get(a => a.Id == createReportRequest.TargetId).FirstOrDefault();
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

            var reportResponse = _mapper.Map<ReportResponse>(newReport);
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
