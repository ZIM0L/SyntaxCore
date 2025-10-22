using System.Net;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using SendGrid.Helpers.Errors.Model;

namespace SyntaxCore.Infrastructure.Middlewares
{
    public class GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
    {
        private readonly RequestDelegate _next = next ?? throw new ArgumentNullException(nameof(next));
        private readonly ILogger<GlobalExceptionMiddleware> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception exception)
            {
                await HandleExceptionAsync(context, exception);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            HttpStatusCode status;
            string errorMessage = exception.Message;
            status = exception switch
            {
                UnauthorizedAccessException => HttpStatusCode.Unauthorized,
                BadHttpRequestException => HttpStatusCode.BadRequest,
                ArgumentNullException => HttpStatusCode.BadRequest,
                ArgumentException => HttpStatusCode.BadRequest,
                NotFoundException => HttpStatusCode.NotFound,
                ForbiddenException => HttpStatusCode.Forbidden,
                ValidationException => HttpStatusCode.BadRequest,
                DbUpdateConcurrencyException => HttpStatusCode.Conflict,
                _ => HttpStatusCode.InternalServerError
            };

            context.Response.StatusCode = (int)status;
            var errorResponse = new
            {
                Success = false,
                Message = errorMessage,
                StatusCode = context.Response.StatusCode,
                ErrorDetails = exception.StackTrace,
                Detail = exception.Message
            };
            _logger.LogError(exception, errorResponse.Message);
            await context.Response.WriteAsJsonAsync(errorResponse);
        }
    }

}
