using DentalApp.Application.Common.Interfaces;
using DentalApp.Application.Features.Appointments.Queries.GetAllAppointments;
using DentalApp.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalApp.Application.Features.Appointments.Queries.AppointmentsByPatientId
{
    public class AppointmentsByPatientIdQueryHandler : IRequestHandler<GetCompletedAppointmentsQuery, AppointmentsByPatientIdResponse>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        public AppointmentsByPatientIdQueryHandler(IApplicationDbContext applicationDbContext, ICurrentUserService currentUserService)
        {
            _context = applicationDbContext;
            _currentUserService = currentUserService;
        }
        public async Task<AppointmentsByPatientIdResponse> Handle(GetCompletedAppointmentsQuery request, CancellationToken cancellationToken)
        {
            var currentDoctorId = _currentUserService.DoctorId;
            if (currentDoctorId == null)
            {
               
                throw new UnauthorizedAccessException("Randevuları listelemek için geçerli bir doktor oturumu bulunamadı.");
            }
            var appointments = await _context.Appointments
          .AsNoTracking()
          .Where(a => a.PatientId == request.PatientId &&
                      a.DoctorId == currentDoctorId.Value &&
                      a.Status == AppointmentStatus.Tamamlandı)
          .OrderByDescending(a => a.Date)
          .Select(a => new CompletedAppointmentDto(
              a.Id,
              a.Date,
              a.CompletionNotes
          ))
          .ToListAsync(cancellationToken);
            return new AppointmentsByPatientIdResponse { CompletedAppointments = appointments };

        }
    }
}
 