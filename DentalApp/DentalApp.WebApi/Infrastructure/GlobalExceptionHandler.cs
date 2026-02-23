using DentalApp.Application.Common.Exceptions;
using DentalApp.Domain.DomainExceptionHandler;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace DentalApp.WebApi.Infrastructure
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            _logger.LogError(exception, "Hata oluştu: {Message}", exception.Message);

            var problemDetails = new ProblemDetails
            {
                Instance = httpContext.Request.Path
            };

            if (exception is ValidationException validationException)
            {
                httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                problemDetails.Title = "Doğrulama Hatası";
                problemDetails.Detail = "Bir veya daha fazla validasyon hatası oluştu.";
                problemDetails.Status = StatusCodes.Status400BadRequest;
                problemDetails.Extensions["errors"] = validationException.Errors;
            }
            else if (exception is NotFoundException)
            {
                httpContext.Response.StatusCode = StatusCodes.Status404NotFound;
                problemDetails.Title = "Bulunamadı";
                problemDetails.Detail = exception.Message;
                problemDetails.Status = StatusCodes.Status404NotFound;
            }
            else if (exception is DomainException) //Domainde kendi türettiğimiz hata türünden olan hataları açık olarak gösteriyoruz.
            {
                httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                problemDetails.Title = "Geçersiz İşlem";
                problemDetails.Detail = exception.Message; // <--- Mesajı burada açıyoruz
                problemDetails.Status = StatusCodes.Status400BadRequest;
            }
            else
            {
                // Beklenmeyen Hata (500)
                httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
                problemDetails.Title = "Sunucu Hatası";
                problemDetails.Detail = "Beklenmeyen bir hata oluştu."; // Güvenlik için gerçek hatayı gizliyoruz         
                problemDetails.Status = StatusCodes.Status500InternalServerError;
            }

            httpContext.Response.ContentType = "application/problem+json";
            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

            return true;
        }
    }
}