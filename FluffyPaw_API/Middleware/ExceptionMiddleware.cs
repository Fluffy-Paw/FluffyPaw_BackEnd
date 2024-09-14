using FluffyPaw_Domain.CustomException;
using Microsoft.AspNetCore.Authorization;
using System.Net;
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
