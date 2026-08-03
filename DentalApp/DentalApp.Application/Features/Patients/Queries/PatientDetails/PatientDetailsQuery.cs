using DentalApp.Application.Features.Patients.Queries.GetPatientImages;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalApp.Application.Features.Patients.Queries.PatientDetails
{
    public record PatientDetailsQuery(Guid PatientId) : IRequest<PatientDetailsResponse>;
 
}
