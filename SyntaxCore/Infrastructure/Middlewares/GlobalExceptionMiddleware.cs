using System.Net;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using SyntaxCore.Infrastructure.ErrorExceptions;

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
                var exceptionToHandle = (exception is AggregateException aggEx)
                    ? aggEx.Flatten().InnerExceptions.FirstOrDefault() ?? exception
                    : exception;

                await HandleExceptionAsync(context, exceptionToHandle);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            if (context.Response.HasStarted)
            {
                _logger.LogWarning("Response has already started, cannot modify headers for exception: {Message}", exception.Message);
                return;
            }
            context.Response.ContentType = "application/json";
            string errorMessage = exception.Message;
            var status = exception switch
            {
                ArgumentException => HttpStatusCode.BadRequest,
                NotFoundException => HttpStatusCode.NotFound,
                ForbiddenException => HttpStatusCode.Forbidden,
                ValidationException => HttpStatusCode.BadRequest,
                DbUpdateConcurrencyException => HttpStatusCode.Conflict,
                _ => HttpStatusCode.InternalServerError
            };
            if (status == HttpStatusCode.InternalServerError)
            {
                _logger.LogError(exception, "Unhandled exception occurred");
            }

            context.Response.StatusCode = (int)status;
            var errorResponse = new
            {
                Success = false,
                StatusCode = context.Response.StatusCode,
                Detail = exception.Message
            };
            _logger.LogError(exception, "Exception occurred");
            await context.Response.WriteAsJsonAsync(errorResponse);
        }
    }

}
