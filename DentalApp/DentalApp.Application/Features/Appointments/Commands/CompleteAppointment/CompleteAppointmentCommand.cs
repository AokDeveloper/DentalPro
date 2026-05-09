using DentalApp.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalApp.Application.Features.Appointments.Commands.CompleteAppointment
{
  
    public record CompleteAppointmentCommand(Guid Id, string? Notes) : IRequest;

    public class CompleteAppointmentCommandHandler : IRequestHandler<CompleteAppointmentCommand>
    {
        private readonly IApplicationDbContext _context;
        public CompleteAppointmentCommandHandler(IApplicationDbContext context)
        {
            _context = context; 
        }

        public async Task Handle(CompleteAppointmentCommand request, CancellationToken cancellationToken)
        {
            var appointment = await _context.Appointments
                .FirstOrDefaultAsync(x=> x.Id == request.Id, cancellationToken);
            if (appointment == null)
            {
                throw new Exception("Randevu Bulunamadı");
            }
            
            appointment.Complete(request.Notes);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
