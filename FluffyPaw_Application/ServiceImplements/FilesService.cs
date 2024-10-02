using AutoMapper;
using FluffyPaw_Application.DTO.Request.FilesRequest;
using FluffyPaw_Application.DTO.Response.FilesResponse;
using FluffyPaw_Application.Services;
using FluffyPaw_Domain.CustomException;
using FluffyPaw_Domain.Entities;
using FluffyPaw_Domain.Interfaces;
using FluffyPaw_Domain.Utils;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Application.ServiceImplements
{
    public class FilesService : IFilesService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IFirebaseConfiguration _firebaseConfiguration;

        public FilesService(IUnitOfWork unitOfWork, IMapper mapper, IFirebaseConfiguration firebaseConfiguration)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _firebaseConfiguration = firebaseConfiguration;
        }

        public async Task<List<FileResponse>> UploadFile(FileRequest fileRequest)
        {
            var newFilesList = new List<FileResponse>();

            foreach (var file in fileRequest.File)
            {
                if (file != null)
                {
                    if (file.Length >= 10 * 1024 * 1024)
                    {
                        throw new CustomException.InvalidDataException("Kích thước tập tin vượt quá giới hạn tối đa cho phép.");
                    }

                    string imageDownloadUrl = await _firebaseConfiguration.UploadImage(file);

                    var newFile = _mapper.Map<Files>(fileRequest);
                    newFile.File = imageDownloadUrl;
                    newFile.CreateDate = CoreHelper.SystemTimeNow;
                    newFile.Status = true;

                    _unitOfWork.FilesRepository.Insert(newFile);
                    _unitOfWork.Save();

                    var fileResponse = _mapper.Map<FileResponse>(newFile);

                    newFilesList.Add(fileResponse);
                }
            }


            return newFilesList;
        }

        public async Task<List<FileResponse>> UploadImageOnly(FileRequest fileRequest)
        {
            var newFilesList = new List<FileResponse>();

            var newFile = _mapper.Map<Files>(fileRequest);

            foreach (var file in fileRequest.File)
            {
                if (file != null)
                {
                    string[] imgExtensions = { ".jpg", ".jpeg", ".png", ".gif" };
                    if (!imgExtensions.Contains(Path.GetExtension(file.FileName)))
                    {
                        throw new CustomException.InvalidDataException("Chỉ nhận tệp hình ảnh");
                    }
                    if (file.Length >= 10 * 1024 * 1024)
                    {
                        throw new CustomException.InvalidDataException("Kích thước tập tin vượt quá giới hạn tối đa cho phép.");
                    }

                    string imageDownloadUrl = await _firebaseConfiguration.UploadImage(file);

                    newFile.File = imageDownloadUrl;
                    newFile.CreateDate = CoreHelper.SystemTimeNow;
                    newFile.Status = true;

                    _unitOfWork.FilesRepository.Insert(newFile);
                    _unitOfWork.Save();

                    var fileResponse = _mapper.Map<FileResponse>(newFile);

                    newFilesList.Add(fileResponse);
                }
            }

            return newFilesList;
        }

        public async Task<string> UploadIdentification(IFormFile file)
        {
            string[] imgExtensions = { ".jpg", ".jpeg", ".png", ".gif" };
            if (!imgExtensions.Contains(Path.GetExtension(file.FileName)))
            {
                throw new CustomException.InvalidDataException("Chỉ nhận tệp hình ảnh");
            }
            if (file.Length >= 10 * 1024 * 1024)
            {
                throw new CustomException.InvalidDataException("Kích thước tập tin vượt quá giới hạn tối đa cho phép.");
            }

            string imageDownloadUrl = await _firebaseConfiguration.UploadImage(file);

            return imageDownloadUrl;
        }
    }
}
