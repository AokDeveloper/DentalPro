using DentalApp.Application.Features.Patients.Queries.GetPatientImages;
using MediatR;
using FE = FastEndpoints;

namespace DentalApp.WebApi.Endpoints.Patients
{
    // İstek Modeli (Route'dan ID alacağız)
    public class GetImagesRequest
    {
        public Guid Id { get; set; } // URL'deki {id} buraya eşleşecek
    }

    public class GetImages : FE.Endpoint<GetImagesRequest, GetPatientImagesResponse>
    {
        private readonly ISender _sender;

        public GetImages(ISender sender)
        {
            _sender = sender;
        }

        public override void Configure()
        {
            Get("/api/patients/{id}/images"); // {id} parametresine dikkat
            AllowAnonymous();
        }

        public override async Task HandleAsync(GetImagesRequest req, CancellationToken ct)
        {
            var query = new GetPatientImagesQuery(req.Id);
            var result = await _sender.Send(query, ct);

            await SendOkAsync(result, ct);
        }
    }
}