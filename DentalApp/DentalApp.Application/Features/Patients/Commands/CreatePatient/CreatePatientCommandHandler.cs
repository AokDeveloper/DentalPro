using DentalApp.Application.Common.Interfaces;
using DentalApp.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore; // ToListAsync ve Where için gerekli

namespace DentalApp.Application.Features.Patients.Commands.CreatePatient
{
    public class CreatePatientCommandHandler : IRequestHandler<CreatePatientCommand, Guid>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public CreatePatientCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task<Guid> Handle(CreatePatientCommand request, CancellationToken cancellationToken)
        {
            var currentDoctorId = _currentUserService.DoctorId;

            if (currentDoctorId == null)
            {
                throw new UnauthorizedAccessException("Geçerli bir doktor oturumu bulunamadı. Lütfen tekrar giriş yapın.");
            }

            // 1. Hastayı mevcut constructor ile oluşturuyoruz
            var entity = new Patient(
                request.FirstName,
                request.LastName,
                request.TCKN,
                request.PhoneNumber,
                request.BirthDate,
                currentDoctorId.Value,
                request.SupervisorId
            );

            // 2. EĞER KATEGORİ SEÇİLMİŞSE, ÇOKA ÇOK İLİŞKİYİ KUR
            if (request.SelectedCategoryIds != null && request.SelectedCategoryIds.Any())
            {
                // Gelen ID'lerin veritabanında gerçekten olup olmadığını kontrol ederek getiriyoruz.
                // Bu adım, sahte veya silinmiş bir ID gönderilmesini (Veri Bütünlüğü hatasını) önler.
                var validCategories = await _context.PatientCategories
                    .Where(c => request.SelectedCategoryIds.Contains(c.Id))
                    .ToListAsync(cancellationToken);

                // EF Core bu atamayı gördüğünde, ara tabloya (PatientCategoryAssignments) 
                // otomatik olarak kayıtları INSERT edecektir.
                foreach (var category in validCategories)
                {
                    entity.PatientCategories.Add(category);
                }
            }

            // 3. Hastayı (ve eklenmişse kategorilerini) veritabanına kaydet
            _context.Patients.Add(entity);

            await _context.SaveChangesAsync(cancellationToken);

            return entity.Id;
        }
    }
}