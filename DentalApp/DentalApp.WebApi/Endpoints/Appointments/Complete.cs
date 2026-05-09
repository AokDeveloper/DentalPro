using Amazon.S3.Model;
using DentalApp.Application.Features.Appointments.Commands.CompleteAppointment;
using FastEndpoints;
using MediatR;
using System.Security.Cryptography;

namespace DentalApp.WebApi.Endpoints.Appointments
{
    public class Complete: Endpoint<CompleteAppointmentCommand>
    {
        private readonly IMediator _mediator;
        public Complete(IMediator meditor)
        {
            _mediator = meditor;
        }
        public override void Configure()
        {
            Put("/api/appointments/{id}/complete");
            AllowAnonymous();
            Summary(s =>
            {
                s.Summary = "Randevuyu Tamamla";
                s.Description = "Randevuyu tamamlandı statüsüne çeker ve tedavi sonuç notlarını kaydeder.";
            });
        }
            public override async Task HandleAsync(CompleteAppointmentCommand req, CancellationToken ct)
        {
            await _mediator.Send(req, ct);
            await SendOkAsync(ct);
        
        }
    }
}
