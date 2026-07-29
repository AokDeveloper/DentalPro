using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalApp.Application.Features.Patients.Commands.DeletePatient
{
    public record DeletePatientCommand(Guid Id) : IRequest<bool>;
    
}
