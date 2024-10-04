﻿using FluffyPaw_Application.DTO.Request.AdminRequest;
using FluffyPaw_Application.DTO.Request.AuthRequest;
using FluffyPaw_Application.DTO.Request.ServiceRequest;
using FluffyPaw_Application.DTO.Response.ServiceResponse;
using FluffyPaw_Application.DTO.Response.BrandResponse;
using FluffyPaw_Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Application.Services
{
    public interface IAdminService
    {
        Task<bool> CreateAdmin(AdminRequest adminRequest);
        IEnumerable<BrandResponse> GetAllBrandFalse();
        Task<bool> AcceptBrand(long id);
        Task<List<SerResponse>> GetAllServiceFalseByBrandId(long id);
        Task<bool> AcceptBrandService(long id);
        Task<bool> ActiveDeactiveAccount(long userId);
        Task<string> DowngradeReputation(long userId);
    }
}
    