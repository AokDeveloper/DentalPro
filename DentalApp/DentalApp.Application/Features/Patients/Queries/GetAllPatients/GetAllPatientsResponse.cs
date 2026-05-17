using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalApp.Application.Features.Patients.Queries.GetAllPatients
{
    // Listede görünecek özet bilgiler
    public record PatientDto(Guid Id, string FullName, string TCKN, string PhoneNumber, DateOnly BirthDate, Guid? SupervisorId, string SupervisorFullName);

    // API'den dönecek ana cevap
    public record GetAllPatientsResponse
    {
        public List<PatientDto> Patients { get; init; } = new();
    }
}