using FluffyPaw_Domain.CustomException;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text.Json;
using static FluffyPaw_Domain.CustomException.CustomException;

namespace FluffyPaw_API.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                var endpoint = context.GetEndpoint();

                if (endpoint?.Metadata?.GetMetadata<IAuthorizeData>() != null)
                {
                    var authorizeData = endpoint.Metadata.GetMetadata<IAuthorizeData>();
                    if (authorizeData.Roles != null)
                    {
                        var roles = authorizeData.Roles.Split(',');

                        // Lấy token từ header Authorization
                        var authHeader = context.Request.Headers["Authorization"].ToString();
                        if (authHeader != null && authHeader.StartsWith("Bearer "))
                        {
                            var token = authHeader.Substring("Bearer ".Length).Trim();

                            // Decode token
                            var handler = new JwtSecurityTokenHandler();
                            var jwtToken = handler.ReadToken(token) as JwtSecurityToken;
                            // Lấy danh sách role từ token
                            var tokenRoles = jwtToken.Claims
                                .Where(c => c.Type == ClaimTypes.Role || c.Type == "roles")
                                .Select(c => c.Value);

                            // Kiểm tra role
                            if (!roles.Any(role => tokenRoles.Contains(role)))
                            {
                                throw new ForbbidenException("Bạn không có quyền truy cập vào tài nguyên này.");
                            }
                        }
                        else
                        {
                            throw new UnAuthorizedException("Bạn chưa đăng nhập.");
                        }
                    }
                }

                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }


        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var code = HttpStatusCode.InternalServerError;
            string result;

            switch (exception)
            {
                case CustomException.InvalidDataException invalidDataEx:
                    code = HttpStatusCode.BadRequest;
                    result = invalidDataEx.Message;
                    break;
                case CustomException.DataNotFoundException dataNotFoundEx:
                    code = HttpStatusCode.NotFound;
                    result = dataNotFoundEx.Message;
                    break;
                case CustomException.DataExistException dataExistEx:
                    code = HttpStatusCode.Conflict;
                    result = dataExistEx.Message;
                    break;
                case CustomException.UnAuthorizedException unauthorizedEx:
                    code = HttpStatusCode.Unauthorized;
                    result = unauthorizedEx.Message;
                    break;
                case CustomException.ForbbidenException forbiddenEx:
                    code = HttpStatusCode.Forbidden;
                    result = forbiddenEx.Message;
                    break;
                case CustomException.InternalServerErrorException internalServerEx:
                    code = HttpStatusCode.InternalServerError;
                    result = internalServerEx.Message;
                    break;
                default:
                    _logger.LogError(exception, "Đã xảy ra lỗi không xác định.");
                    result = "Đã xảy ra lỗi không xác định.";
                    break;
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;

            var response = new
            {
                statusCode = context.Response.StatusCode,
                message = result
            };

            return context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}
