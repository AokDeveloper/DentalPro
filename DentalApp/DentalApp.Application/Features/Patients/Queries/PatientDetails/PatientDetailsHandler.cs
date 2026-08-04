using DentalApp.Application.Common.Interfaces;
using DentalApp.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalApp.Application.Features.Patients.Queries.PatientDetails
{
    public class PatientDetailsHandler : IRequestHandler<PatientDetailsQuery, PatientDetailsResponse>
    {
        private readonly IApplicationDbContext _context;
        public PatientDetailsHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<PatientDetailsResponse> Handle(PatientDetailsQuery request, CancellationToken cancellationToken)
        {
           var patientDetail = await _context.Patients
                .AsNoTracking()
                .Where(p=> p.Id== request.PatientId)
                .Select(p=> new PatientDetailDto(
                    p.Id,
                    p.FirstName,
                    p.LastName,
                    p.TCKN,
                    p.PhoneNumber,
                    p.BirthDate,
                    p.PatientNotes,
                    p.Supervisor != null ? $"{p.Supervisor.FullName} " : "Atanmadı",

                    p.TreatmentImages
                        .Where(img => img.Type == TreatmentImageType.Profile) // Enum adını kendi koduna göre uyarla
                        .Select(img => img.ImageUrl)
                        .FirstOrDefault(),

                    p.PatientCategories
                        .Select(c => new PatientDetailCategoryDto(c.Id, c.Name))
                        .ToList()
                    )).FirstOrDefaultAsync(cancellationToken);
            if (patientDetail == null)
            {
                // Projendeki hata yönetimine göre burayı değiştirebilirsin (örn: throw new NotFoundException(...))
                throw new Exception("Hasta bulunamadı.");
            }

            return new PatientDetailsResponse
            {
                Patient = patientDetail
            };
        }
    }
}
