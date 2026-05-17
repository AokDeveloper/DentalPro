using DentalApp.Application.Features.Patients.Queries.GetAllPatients;
using DentalApp.Application.Features.Supervisors.Queries;
using FastEndpoints;
using MediatR;

namespace DentalApp.WebApi.Endpoints.Supervisors
{
    public class GetList : EndpointWithoutRequest<GetAllSupervisorsResponse>
    {
        private readonly ISender _sender;

        public GetList(ISender sender)
        {
            _sender = sender;
        }

        public override void Configure()
        {
            Get("/api/supervisors");
            AllowAnonymous();
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            // Query'i gönder
            var result = await _sender.Send(new GetAllSupervisorsQuery(), ct);

            // Cevabı dön
            await SendOkAsync(result, ct);
        }
    }
}