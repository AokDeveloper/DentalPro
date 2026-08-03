using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalApp.Application.Features.Patients.Queries.PatientDetails
{
    public record PatientDetailCategoryDto(Guid Id, string Name);

    // Hastanın tüm detay bilgilerini ve profil fotoğrafını tutan ana DTO
    public record PatientDetailDto(
        Guid Id,
        string FirstName,
        string LastName,
        string TCKN,
        string PhoneNumber,
        DateOnly BirthDate,       
        string SupervisorFullName,
        string? ProfilePhotoUrl, // Sadece Profil Fotoğrafının MinIO URL'i
        List<PatientDetailCategoryDto> Categories
    );
    public record PatientDetailsResponse
    {
        public PatientDetailDto Patient { get; init; }
    }
}
