using DentalApp.Application.Features.Auth.Commands.Register;
using FastEndpoints;
using MediatR;

namespace DentalApp.WebApi.Endpoints.Auth
{
    public class Register : Endpoint<RegisterCommand, object>
    {
        private readonly ISender _sender;

        public Register(ISender sender)
        {
            _sender = sender;
        }

        public override void Configure()
        {
            Post("/api/auth/register");
            AllowAnonymous(); // <-- Herkes kayıt olabilir
            Summary(s =>
            {
                s.Summary = "Yeni kullanıcı kaydı";
                s.Description = "Ad Soyad, Email ve Şifre (En az 6 karakter) ile kayıt olun.";
            });
        }

        public override async Task HandleAsync(RegisterCommand req, CancellationToken ct)
        {
            // Application katmanındaki Command'i çağır
            var userId = await _sender.Send(req, ct);

            await SendOkAsync(new { message = "Kayıt başarılı", userId = userId }, ct);
        }
    }
}