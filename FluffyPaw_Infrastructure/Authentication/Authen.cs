using Firebase.Auth;
using FluffyPaw_Domain.CustomException;
using FluffyPaw_Domain.Entities;
using FluffyPaw_Domain.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Infrastructure.Authentication
{
    public class Authen : IAuthentication
    {
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<Authen> _logger;
        private readonly IHttpContextAccessor _contextAccessor;

        public Authen(IUnitOfWork unitOfWork, IConfiguration configuration, ILogger<Authen> logger, IHttpContextAccessor contextAccessor)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _logger = logger;
            _contextAccessor = contextAccessor;
        }

        public string GenerateJWTToken(Account account)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, account.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("id", account.Id.ToString()),
                new Claim(ClaimTypes.Role, string.Join(",", account.RoleName)),
                new Claim("username", account.Username),
                new Claim("avatar",account.Avatar)
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public long GetUserIdFromHttpContext(HttpContext httpContext)
        {
            if (!httpContext.Request.Headers.ContainsKey("Authorization"))
            {
                throw new CustomException.InternalServerErrorException("Authorization header is missing.");
            }

            string authorizationHeader = httpContext.Request.Headers["Authorization"];

            if (string.IsNullOrWhiteSpace(authorizationHeader) || !authorizationHeader.StartsWith("Bearer "))
            {
                throw new CustomException.InternalServerErrorException("Invalid Authorization header format.");
            }

            string jwtToken = authorizationHeader["Bearer ".Length..];

            var tokenHandler = new JwtSecurityTokenHandler();
            if (!tokenHandler.CanReadToken(jwtToken))
            {
                throw new CustomException.InternalServerErrorException("Invalid JWT token format.");
            }

            try
            {
                var token = tokenHandler.ReadJwtToken(jwtToken);
                var idClaim = token.Claims.FirstOrDefault(claim => claim.Type == "id");

                if (idClaim == null || string.IsNullOrWhiteSpace(idClaim.Value))
                {
                    throw new CustomException.InternalServerErrorException("User ID claim not found in token.");
                }

                return long.Parse(idClaim.Value);
            }
            catch (Exception ex)
            {
                throw new CustomException.InternalServerErrorException($"Error parsing token: {ex.Message}");
            }
        }
    }
}
