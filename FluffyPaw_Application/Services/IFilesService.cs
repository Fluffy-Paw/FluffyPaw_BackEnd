using FluffyPaw_Application.DTO.Request.FilesRequest;
using FluffyPaw_Application.DTO.Response.FilesResponse;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Application.Services
{
    public interface IFilesService
    {
        Task<List<FileResponse>> UploadImageOnly(FileRequest fileRequest);
        Task<List<FileResponse>> UploadFile(FileRequest fileRequest);
        Task<string> UploadIdentification(IFormFile file);
    }
}
