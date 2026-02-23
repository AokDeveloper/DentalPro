using DentalApp.Application.Features.Patients.Queries.GetAllPatients;
using MediatR;
using FastEndpoints;

namespace DentalApp.WebApi.Endpoints.Patients
{
    public class GetList : EndpointWithoutRequest<GetAllPatientsResponse>
    {
        private readonly ISender _sender;

        public GetList(ISender sender)
        {
            _sender = sender;
        }

        public override void Configure()
        {
            Get("/api/patients");
            AllowAnonymous();
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            // Query'i gönder
            var result = await _sender.Send(new GetAllPatientsQuery(), ct);

            // Cevabı dön
            await SendOkAsync(result, ct);
        }
    }
}