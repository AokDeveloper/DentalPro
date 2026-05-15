using DentalApp.Domain.Enums;
using MediatR;


namespace DentalApp.Application.Features.Appointments.Commands.CreateAppointment
{
    public record CreateAppointmentCommand : IRequest<Guid>
    {
        public Guid PatientId { get;  init; }
        public DateTime Date { get;  init; }      
        public string? Notes { get;  init; }
        public Guid DoctorId { get; init; }
        public int Duration { get; set; }
        public bool IsImportant { get; set; }
    }
}
