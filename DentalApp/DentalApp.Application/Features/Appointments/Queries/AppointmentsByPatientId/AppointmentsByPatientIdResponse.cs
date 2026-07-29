using DentalApp.Application.Features.Appointments.Queries.GetAllAppointments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalApp.Application.Features.Appointments.Queries.AppointmentsByPatientId
{
   public record  CompletedAppointmentDto(
        Guid Id,       
        DateTime Date,       
        string? CompletionNotes
       
    );
    public record AppointmentsByPatientIdResponse
    {
        public List<CompletedAppointmentDto> CompletedAppointments { get; init; } = new();
    }
}
