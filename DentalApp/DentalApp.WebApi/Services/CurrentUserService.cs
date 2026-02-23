using DentalApp.Application.Common.Interfaces;
using System.Security.Claims;

namespace DentalApp.WebApi.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string? UserId
        {
            get
            {
                // JWT Token içindeki "sub" (Subject) veya "nameidentifier" claim'ini okur.
                // Identity kütüphanesi User ID'yi otomatik olarak bu Claim'e atar.
                var id = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

                // Eğer ID varsa onu dön, yoksa null dön.
                return id;
            }
        }
    }
}