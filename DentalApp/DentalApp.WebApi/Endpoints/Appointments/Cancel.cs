using DentalApp.Application.Features.Appointments.Commands.CancelAppointment;
using FastEndpoints;
using MediatR;

namespace DentalApp.WebApi.Endpoints.Appointments
{
    public class Cancel : Endpoint<CancelAppointmentCommand>
    {
        private readonly IMediator _mediator;
        public Cancel(IMediator mediator)
        {
          _mediator = mediator;  
        }
        public override void Configure()
        {
            Put("/api/appointments/{id}/cancel");
            AllowAnonymous();
            Summary(s =>
            {
                s.Summary = "Randevuyu İptal Et";
                s.Description = "Belirtilen randevuyu iptale çeker";

            });
        }
        public override async Task HandleAsync(CancelAppointmentCommand req, CancellationToken ct)
        {
            await _mediator.Send(req, ct);
            await SendOkAsync(ct);
        }
    }
}
