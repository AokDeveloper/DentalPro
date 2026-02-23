using DentalApp.Application.Features.Patients.Commands.CreatePatient;
using FastEndpoints;
using MediatR;

namespace DentalApp.WebApi.Endpoints.Patients
{
    // Endpoint<İstek, Cevap>
    public class Create : Endpoint<CreatePatientCommand, Guid>
    {
        private readonly ISender _sender; // MediatR göndericisi

        public Create(ISender sender)
        {
            _sender = sender;
        }

        public override void Configure()
        {
            Post("/api/patients");
            AllowAnonymous(); // Şimdilik şifresiz
            Summary(s =>
            {
                s.Summary = "Yeni hasta kaydı oluşturur";
                s.Description = "Ad, Soyad, TC ve Telefon ile hasta ekler.";
                s.Response<Guid>(200, "Oluşturulan hastanın ID'si");
            });
        }

        public override async Task HandleAsync(CreatePatientCommand req, CancellationToken ct)
        {
            // İsteği MediatR'a (Application katmanına) gönder
            var result = await _sender.Send(req, ct);

            // Cevabı dön (200 OK + ID)
            await SendAsync(result, cancellation: ct);
        }
    }
}