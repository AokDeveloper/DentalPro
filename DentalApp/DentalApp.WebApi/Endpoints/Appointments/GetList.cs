using DentalApp.Application.Features.Appointments.Queries.GetAllAppointments;
using FastEndpoints;
using MediatR;

namespace DentalApp.WebApi.Endpoints.Appointments
{
    public class GetList : EndpointWithoutRequest<GetAllAppointmentsResponse>
    {
        private readonly IMediator _mediator;

        public GetList(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override void Configure()
        {
            Get("/api/appointments");
            //AllowAnonymous();
            Summary(s =>
            {
                s.Summary = "Randevuları Listele";
                s.Description = "Tüm randevuları hasta isimleri ve durumlarıyla birlikte getirir.";
            });
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            // Query'yi çalıştır
            var result = await _mediator.Send(new GetAllAppointmentsQuery(), ct);

            // Sonucu dön
            await SendOkAsync(result, ct);
        }
    }
}