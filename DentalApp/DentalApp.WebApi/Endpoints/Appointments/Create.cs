using DentalApp.Application.Features.Appointments.Commands.CreateAppointment;
using DentalApp.Application.Features.Patients.Commands.CreatePatient;
using FastEndpoints;
using MediatR;

namespace DentalApp.WebApi.Endpoints.Appointments
{
    public class Create : Endpoint<CreateAppointmentCommand, Guid>
    {
        private readonly ISender _sender; // MediatR göndericisi

        public Create(ISender sender)
        {
            _sender = sender;
        }

        public override void Configure()
        {
            Post("/api/appointments");
            AllowAnonymous(); // Test için açık, sonra Roles("Admin") yapabilirsin
            Summary(s =>
            {
                s.Summary = "Yeni randevu oluşturur";
                s.Description = "Hasta ID ve tarih bilgisiyle randevu kaydı açar.";
            });
        }

        public override async Task HandleAsync(CreateAppointmentCommand req, CancellationToken ct)
        {
            // İsteği MediatR'a (Application katmanına) gönder
            var result = await _sender.Send(req, ct);

            // Cevabı dön (200 OK + ID)
            await SendAsync(result, cancellation: ct);
        }
    }
}


