using DentalApp.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalApp.Application.Features.Appointments.Commands.DidntComeAppointment
{

    public record DidntComeAppointmentCommand(Guid Id, string Notes) : IRequest;

    public class DidntComeAppointmentCommandHandler : IRequestHandler<DidntComeAppointmentCommand>
    {
        private readonly IApplicationDbContext _context;
        public DidntComeAppointmentCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task Handle(DidntComeAppointmentCommand request, CancellationToken cancellationToken)
        {
            var appointment = await _context.Appointments
                 .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (appointment == null)
            {
                throw new Exception("Randevu bulunamadı");
            }

            appointment.DidntCome(request.Notes);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
