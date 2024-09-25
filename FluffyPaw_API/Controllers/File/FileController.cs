using CoreApiResponse;
using FluffyPaw_Application.DTO.Request.AdminRequest;
using FluffyPaw_Application.DTO.Request.FilesRequest;
using FluffyPaw_Application.ServiceImplements;
using FluffyPaw_Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FluffyPaw_API.Controllers.File
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : BaseController
    {
        private readonly IFilesService _filesService;

        public FileController(IFilesService filesService)
        {
            _filesService = filesService;
        }

        [HttpPost("UploadFile")]
        public async Task<IActionResult> UploadFile([FromForm] FileRequest fileRequest)
        {
            var file = await _filesService.UploadFile(fileRequest);
            return CustomResult("Tải lên thành công.", file);
        }
    }
}