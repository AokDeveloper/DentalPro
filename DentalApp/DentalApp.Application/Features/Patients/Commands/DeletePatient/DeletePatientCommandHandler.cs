using DentalApp.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalApp.Application.Features.Patients.Commands.DeletePatient
{
    public class DeletePatientCommandHandler : IRequestHandler<DeletePatientCommand, bool>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public DeletePatientCommandHandler(IApplicationDbContext applicationDbContext, ICurrentUserService currentUserService)
        {
            _context = applicationDbContext;
            _currentUserService = currentUserService;
        }
        public async Task<bool> Handle(DeletePatientCommand request, CancellationToken cancellationToken)
        {
           var patient = await _context.Patients.FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

            if (patient == null) throw new Exception("Hasta bulunamadı.");

            // Veri İzolasyonu: Başkasının hastasını silemez
            if (patient.DoctorId != _currentUserService.DoctorId)
            { 
                throw new UnauthorizedAccessException("Sadece size zimmetli hastaları silebilirsiniz.");
            }

            patient.MarkAsDeleted();

            await _context.SaveChangesAsync(cancellationToken);
            return true;

        }
    }
}


