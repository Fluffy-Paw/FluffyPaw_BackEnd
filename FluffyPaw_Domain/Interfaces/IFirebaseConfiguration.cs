using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Domain.Interfaces
{
    public interface IFirebaseConfiguration
    {
        Task<string> UploadImage(IFormFile file);
    }
}
