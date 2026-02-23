using DentalApp.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalApp.Application.Features.Auth.Commands.Register
{
    public record RegisterCommand(string FullName, string Email, string Password) : IRequest<string>;

    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, string>
    {
        private readonly UserManager<AppUser> _userManager;

        public RegisterCommandHandler(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<string> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            // Kullanıcı var mı kontrolü
            var existingUser = await _userManager.FindByEmailAsync(request.Email);
            if (existingUser != null) throw new Exception("Bu email zaten kayıtlı.");

            var user = new AppUser
            {
                UserName = request.Email, // Username email ile aynı olsun
                Email = request.Email,
                FullName = request.FullName
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                throw new Exception("Kayıt başarısız: " + string.Join(", ", result.Errors.Select(e => e.Description)));
            }

            return user.Id;
        }
    }
}