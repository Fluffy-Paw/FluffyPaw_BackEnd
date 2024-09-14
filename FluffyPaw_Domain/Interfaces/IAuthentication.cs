using FluffyPaw_Domain.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Domain.Interfaces
{
    public interface IAuthentication
    {
        string GenerateJWTToken(Account account);
        Guid GetUserIdFromHttpContext(HttpContext httpContext);
    }
}
