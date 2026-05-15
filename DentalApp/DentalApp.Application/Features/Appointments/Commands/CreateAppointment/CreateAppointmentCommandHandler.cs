using DentalApp.Application.Common.Interfaces;
using DentalApp.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace DentalApp.Application.Features.Appointments.Commands.CreateAppointment
{
    public class CreateAppointmentCommandHandler : IRequestHandler<CreateAppointmentCommand, Guid>
    {
        private readonly IApplicationDbContext _context;
        public CreateAppointmentCommandHandler(IApplicationDbContext context)
        {
         _context = context;   
        }

        public async Task<Guid> Handle(CreateAppointmentCommand request, CancellationToken cancellationToken)
        {
            var patientExists = await _context.Patients
                .AnyAsync(p => p.Id == request.PatientId, cancellationToken);

            if (!patientExists)
            {
                // Buraya kendi NotFoundException sınıfını ekleyebilirsin
                throw new Exception($"ID'si {request.PatientId} olan hasta bulunamadı.");
            }

            var entity = new Appointment(
                request.PatientId,
                request.Date,
                request.Notes,
                request.DoctorId,
                request.Duration,
                request.IsImportant
                );
            _context.Appointments.Add( entity );
            await _context.SaveChangesAsync(cancellationToken);
            return entity.Id;
        }
    }
}


