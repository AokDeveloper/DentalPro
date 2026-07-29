using DentalApp.Application.Features.Patients.Queries.GetAllPatients;
using DentalApp.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalApp.Application.Features.Appointments.Queries.GetAllAppointments
{
    public record AppointmentDto(
     Guid Id,
     Guid PatientId,
     string DoctorName,
     string PatientName,
     DateTime Date,
     AppointmentStatus Status,   
     string? Notes,
     string? CompletionNotes,
     int Duration,
     bool IsImportant

 );
    public record GetAllAppointmentsResponse
    {
        public List<AppointmentDto> Appointments { get; init; } = new();
    }
}

