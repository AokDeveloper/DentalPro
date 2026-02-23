using DentalApp.Application.Common.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalApp.Application.Features.Appointments.Queries
{    
    public record GetAllAppointmentsQuery : IRequest<GetAllAppointmentsResponse>, ILoggableQuery;
}
