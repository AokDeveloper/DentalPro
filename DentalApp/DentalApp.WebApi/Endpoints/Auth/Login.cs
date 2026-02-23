using DentalApp.Application.Features.Auth.Commands.Login;
using FastEndpoints;
using MediatR;

namespace DentalApp.WebApi.Endpoints.Auth
{
    public class Login : Endpoint<LoginCommand, LoginResponse>
    {
        private readonly ISender _sender;

        public Login(ISender sender)
        {
            _sender = sender;
        }

        public override void Configure()
        {
            Post("/api/auth/login");
            AllowAnonymous(); // <-- Giriş yapmamış kişi çağıracak
            Summary(s =>
            {
                s.Summary = "Sisteme giriş yap";
                s.Description = "Email ve şifre ile JWT Token alın.";
            });
        }

        public override async Task HandleAsync(LoginCommand req, CancellationToken ct)
        {
            // LoginResponse içinde 'Token' ve 'FullName' var
            var response = await _sender.Send(req, ct);

            await SendOkAsync(response, ct);
        }
    }
}