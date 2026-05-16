using DentalApp.Application.Common.Interfaces;
using DentalApp.Domain.Entities;
using MediatR;

namespace DentalApp.Application.Features.Patients.Commands.CreatePatient
{
    public class CreatePatientCommandHandler : IRequestHandler<CreatePatientCommand, Guid>
    {
        private readonly IApplicationDbContext _context;

        public CreatePatientCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> Handle(CreatePatientCommand request, CancellationToken cancellationToken)
        {
            // Entity oluştur (Domain kuralları burada çalışır)
            var entity = new Patient(
                request.FirstName,
                request.LastName,
                request.TCKN,
                request.PhoneNumber,
                request.BirthDate,
                request.DoctorId
            );

            // Veritabanına ekle
            _context.Patients.Add(entity);

            // Kaydet
            await _context.SaveChangesAsync(cancellationToken);

            // Yeni oluşan ID'yi döndür
            return entity.Id;
        }
    }
}