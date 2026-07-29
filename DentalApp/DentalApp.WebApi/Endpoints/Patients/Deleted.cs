using DentalApp.Application.Features.Patients.Commands.DeletePatient;
using FastEndpoints;
using MediatR;

namespace DentalApp.WebApi.Endpoints.Patients
{
    public class Deleted : Endpoint<DeletePatientCommand>
    {
        private readonly IMediator _mediator;

        public Deleted(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override void Configure()
        {
            Delete("/api/patients/{id}");
            //AllowAnonymous();
            Summary(s =>
            {
                s.Summary = "Hastayı Sil (Soft Delete)";
                s.Description = "Verilen ID'ye ait hastayı sistemden silinmiş olarak işaretler.";
              
            });
        }

        public override async Task HandleAsync(DeletePatientCommand req, CancellationToken ct)
        {
            
            await _mediator.Send(req, ct);

            // Başarılı olursa 204 No Content dön
            await SendNoContentAsync(ct);
        }
    }
}