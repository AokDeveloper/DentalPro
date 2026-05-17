using DentalApp.Application.Common.Interfaces;
using DentalApp.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace DentalApp.Application.Features.Appointments.Commands.CreateAppointment
{
    public class CreateAppointmentCommandHandler : IRequestHandler<CreateAppointmentCommand, Guid>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        public CreateAppointmentCommandHandler(IApplicationDbContext context,ICurrentUserService currentUserService)
        {
         _context = context;   
         _currentUserService = currentUserService;
        }

        public async Task<Guid> Handle(CreateAppointmentCommand request, CancellationToken cancellationToken)
        {
            var currentDoctorId = _currentUserService.DoctorId;


            if (currentDoctorId == null)
            {
                throw new UnauthorizedAccessException("Geçerli bir doktor oturumu bulunamadı. Lütfen tekrar giriş yapın.");
            }

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
                 currentDoctorId.Value,
                request.Duration,
                request.IsImportant
                );
            _context.Appointments.Add( entity );
            await _context.SaveChangesAsync(cancellationToken);
            return entity.Id;
        }
    }
}


