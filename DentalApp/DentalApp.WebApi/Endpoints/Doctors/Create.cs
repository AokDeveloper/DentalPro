using DentalApp.Application.Features.Doctors.Commands.CreateDoctor;
using FastEndpoints;
using MediatR;

namespace DentalApp.WebApi.Endpoints.Doctors
{
    // Endpoint<İstek, Cevap>
    public class Create : Endpoint<CreateDoctorCommand, Guid>
    {
        private readonly ISender _sender; // MediatR göndericisi

        public Create(ISender sender)
        {
            _sender = sender;
        }

        public override void Configure()
        {
            Post("/api/doctors");
            AllowAnonymous(); // Şimdilik şifresiz
            Summary(s =>
            {
                s.Summary = "Yeni doktor kaydı oluşturur";
                s.Description = "Ad, Soyad, TC doktor ekler.";
                s.Response<Guid>(200, "Oluşturulan doktorun ID'si");
            });
        }

        public override async Task HandleAsync(CreateDoctorCommand req, CancellationToken ct)
        {
            // İsteği MediatR'a (Application katmanına) gönder
            var result = await _sender.Send(req, ct);

            // Cevabı dön (200 OK + ID)
            await SendAsync(result, cancellation: ct);
        }
    }
}