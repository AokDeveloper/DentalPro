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
        public Guid? DoctorId // <--- Tipi Guid? olarak güncellendi
        {
            get
            {
                var doctorIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirstValue("DoctorId");

                // Eğer claim boş değilse ve geçerli bir Guid formatındaysa çevirip döndür
                if (!string.IsNullOrEmpty(doctorIdClaim) && Guid.TryParse(doctorIdClaim, out Guid parsedId))
                {
                    return parsedId;
                }

                // Token'da yoksa veya dönüştürülemezse null dön
                return null;
            }
        }

    }
}