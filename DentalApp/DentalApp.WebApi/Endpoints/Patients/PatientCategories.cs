using DentalApp.Application.Features.Patients.Queries.PatientCategories;
using FastEndpoints;
using MediatR;

namespace DentalApp.WebApi.Endpoints.Patients
{
    public class PatientCategories : EndpointWithoutRequest<PatientCategoriesResponse>
    {
        private readonly IMediator _mediator;

        public PatientCategories(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override void Configure()
        {
            Get("api/patients/categories/grouped");

            Summary(s =>
            {
                s.Summary = "Kategorileri Gruplu Listele";
                s.Description = "Angular Grouped MultiSelect bileşeni için hiyerarşik kategori listesi döner.";
            });
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            // Query'mizi gönderiyoruz, geriye GetGroupedCategoriesResponse dönüyor
            var result = await _mediator.Send(new PatientCategoriesQuery(), ct);

            await SendOkAsync(result, ct);
        }
    }
}