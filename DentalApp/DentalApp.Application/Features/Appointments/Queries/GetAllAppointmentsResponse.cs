using DentalApp.Application.Features.Appointments.Queries;
using DentalApp.Application.Features.Patients.Queries.GetAllPatients;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalApp.Application.Features.Appointments.Queries
{
    public record AppointmentDto(
     Guid Id,
     Guid PatientId,
     string DoctorName,
     string PatientName,
     DateTime Date,     
     string Status,
     string? Notes
 );
    public record GetAllAppointmentsResponse
    {
        public List<AppointmentDto> Appointments { get; init; } = new();
    }
}

