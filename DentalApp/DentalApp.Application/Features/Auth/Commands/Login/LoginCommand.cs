using DentalApp.Application.Common.Interfaces;
using DentalApp.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalApp.Application.Features.Auth.Commands.Login
{
    public record LoginCommand(string Email, string Password) : IRequest<LoginResponse>;
    public record LoginResponse(string Token, string FullName);

    public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponse>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        // 1. VERİTABANI BAĞLANTISINI EKLİYORUZ (Sizin DbContext veya Repository adınız neyse onu yazın)
        private readonly IApplicationDbContext _context;

        public LoginCommandHandler(
            UserManager<AppUser> userManager,
            IJwtTokenGenerator jwtTokenGenerator,
            IApplicationDbContext context) // Constructor'a eklendi
        {
            _userManager = userManager;
            _jwtTokenGenerator = jwtTokenGenerator;
            _context = context;
        }

        public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null) throw new Exception("Giriş başarısız.");

            var checkPassword = await _userManager.CheckPasswordAsync(user, request.Password);
            if (!checkPassword) throw new Exception("Giriş başarısız.");

            // 2. DOKTORU BULUYORUZ
            // Kullanıcı (AppUser) ID'si ile Doctors tablosundaki eşleşen kaydı arıyoruz.
            // (Eğer sizin Doctors tablonuzda Kullanıcı ID'sini tutan alanın adı farklıysa 'AppUserId' kısmını değiştirin)
            var doctor = await _context.Doctors
                .FirstOrDefaultAsync(d => d.AppUserId == user.Id, cancellationToken);

            // Doktor bulunduysa gerçek ID'sini al, bulunamadıysa null bırak (belki sekreter giriş yapıyordur)
            string doctorId = doctor != null ? doctor.Id.ToString() : null;

            // 3. BULUNAN DOKTOR ID'SİNİ TOKEN ÜRETİCİSİNE GÖNDERİYORUZ
            var token = _jwtTokenGenerator.GenerateToken(user, doctorId);

            return new LoginResponse(token, user.FullName);
        }
    }
}