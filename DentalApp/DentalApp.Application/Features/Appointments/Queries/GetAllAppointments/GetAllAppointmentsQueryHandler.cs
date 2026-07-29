using DentalApp.Application.Common.Interfaces;
using DentalApp.Application.Features.Patients.Queries.GetAllPatients;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DentalApp.Application.Features.Appointments.Queries.GetAllAppointments
{
    public class GetAllAppointmentsQueryHandler : IRequestHandler<GetAllAppointmentsQuery, GetAllAppointmentsResponse>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        public GetAllAppointmentsQueryHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }
        public async Task<GetAllAppointmentsResponse> Handle(GetAllAppointmentsQuery request, CancellationToken cancellationToken)
        {

            var currentDoctorId = _currentUserService.DoctorId;
            if (currentDoctorId == null)
            {
                // Güvenlik: Eğer token yoksa veya bozuksa boş liste dön veya hata fırlat
                throw new UnauthorizedAccessException("Randevuları listelemek için geçerli bir doktor oturumu bulunamadı.");
            }

            var appointments =  await _context.Appointments
                .AsNoTracking()
                .Include(a => a.Patient) // ProjectToType mapper include yazmasak bile dto içinde patientname var diye otomatik algılar ve tabloyu joinler
                .Include(a => a.Doctor)
                .Where(p => p.DoctorId == currentDoctorId.Value)
                .OrderBy(a => a.Date)
                .ProjectToType<AppointmentDto>()
                .ToListAsync(cancellationToken);
            return new GetAllAppointmentsResponse { Appointments = appointments };
        }
    }
}

