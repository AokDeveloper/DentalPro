using DentalApp.Application.Features.Appointments.Commands.DidntComeAppointment;
using FastEndpoints;
using MediatR;

namespace DentalApp.WebApi.Endpoints.Appointments
{
    public class DidntCome : Endpoint<DidntComeAppointmentCommand>
    {
        private readonly IMediator _mediator;
        public DidntCome(IMediator mediator)
        {
            _mediator = mediator;
        }
        public override void Configure()
        {
            Put("/api/appointments/{id}/didntcome");
            AllowAnonymous();
            Summary(s =>
            {
                s.Summary = "Randevuyu Gelmedi Olarak Güncelle";
                s.Description = "Belirtilen randevuyu gelmedi olarak günceller";

            });
        }
        public override async Task HandleAsync(DidntComeAppointmentCommand req, CancellationToken ct)
        {
            await _mediator.Send(req, ct);
            await SendOkAsync(ct);
        }
    }
}
