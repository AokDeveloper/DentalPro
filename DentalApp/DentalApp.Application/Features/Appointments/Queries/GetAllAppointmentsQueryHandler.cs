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

namespace DentalApp.Application.Features.Appointments.Queries
{
    public class GetAllAppointmentsQueryHandler : IRequestHandler<GetAllAppointmentsQuery, GetAllAppointmentsResponse>
    {
        private readonly IApplicationDbContext _context;
        public GetAllAppointmentsQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<GetAllAppointmentsResponse> Handle(GetAllAppointmentsQuery request, CancellationToken cancellationToken)
        {
          var appointments =  await _context.Appointments
                .AsNoTracking()
                .Include(a => a.Patient) // ProjectToType mapper include yazmasak bile dto içinde patientname var diye otomatik algılar ve tabloyu joinler
                .Include(a => a.Doctor)
                .OrderByDescending(a => a.Date)
                .ProjectToType<AppointmentDto>()
                .ToListAsync(cancellationToken);
            return new GetAllAppointmentsResponse { Appointments = appointments };
        }
    }
}

