using DentalApp.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalApp.Application.Features.Appointments.Commands.CancelAppointment
{
       public record CancelAppointmentCommand(Guid Id, string Reason):IRequest;

    public class CancelAppointmentCommandHandler : IRequestHandler<CancelAppointmentCommand>
    {
        private readonly IApplicationDbContext _context;
        public CancelAppointmentCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task Handle(CancelAppointmentCommand request, CancellationToken cancellationToken)
        {
           var appointment = await _context.Appointments
                .FirstOrDefaultAsync(x=> x.Id == request.Id, cancellationToken);

            if (appointment == null)
            {
                throw new Exception("Randevu bulunamadı");
            }
            
            appointment.Cancel(request.Reason);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
