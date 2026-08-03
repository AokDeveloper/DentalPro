using DentalApp.Application.Features.Patients.Queries.PatientDetails;
using FastEndpoints;
using MediatR;

namespace DentalApp.WebApi.Endpoints.Patients
{
    public class PatientDetailRequest
    {
        public Guid PatientId { get; set; }
    }

    // Senin standartına uygun isimlendirme: Endpoint eki yok
    public class PatientDetail : Endpoint<PatientDetailRequest, PatientDetailsResponse>
    {
        private readonly IMediator _mediator;

        public PatientDetail(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override void Configure()
        {
            
            Get("api/patients/{PatientId}/detail");

            Summary(s =>
            {
                s.Summary = "Hasta Detaylarını Getir";
                s.Description = "Hastanın temel bilgilerini, kategorilerini ve profil fotoğrafının URL'ini döner.";
            });
        }

        public override async Task HandleAsync(PatientDetailRequest req, CancellationToken ct)
        {
            // Query'mizi gönderiyoruz, geriye GetPatientDetailResponse dönüyor
            var result = await _mediator.Send(new PatientDetailsQuery(req.PatientId), ct);

            await SendOkAsync(result, ct);
        }
    }
}