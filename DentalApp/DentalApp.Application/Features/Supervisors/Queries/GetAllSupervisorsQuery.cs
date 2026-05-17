using DentalApp.Application.Features.Patients.Queries.GetAllPatients;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalApp.Application.Features.Supervisors.Queries
{
    public record GetAllSupervisorsQuery : IRequest<GetAllSupervisorsResponse>;
}
