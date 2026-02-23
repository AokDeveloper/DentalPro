using DentalApp.Application.Common.Interfaces;
using DentalApp.Domain.Entities;
using DentalApp.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalApp.Application.Features.Patients.Commands.UploadImage
{
    // 1. İSTEK MODELİ
    public class UploadTreatmentImageCommand : IRequest<Guid>
    {
        public Guid PatientId { get; set; }
        public Stream FileStream { get; set; } // Dosyanın kendisi
        public string FileName { get; set; }
        public string ContentType { get; set; } // image/jpeg vb.
        public TreatmentImageType ImageType { get; set; }
    }

    // 2. İŞLEYİCİ
    public class UploadTreatmentImageCommandHandler : IRequestHandler<UploadTreatmentImageCommand, Guid>
    {
        private readonly IApplicationDbContext _context;
        private readonly IFileStorageService _storageService; // MinIO Servisi

        public UploadTreatmentImageCommandHandler(IApplicationDbContext context, IFileStorageService storageService)
        {
            _context = context;
            _storageService = storageService;
        }
        public async Task<Guid> Handle(UploadTreatmentImageCommand request, CancellationToken cancellationToken)
        {
            // 1. Önce böyle bir hasta var mı diye kontrol edelim (Veriyi çekmeye gerek yok, var mı yok mu baksak yeter)
            // AsNoTracking performansı artırır.
            var patientExists = await _context.Patients
                .AsNoTracking()
                .AnyAsync(p => p.Id == request.PatientId, cancellationToken);

            if (!patientExists)
                throw new Exception("Hasta bulunamadı.");

            // 2. Dosya ismini güvenli hale getir
            var fileExtension = Path.GetExtension(request.FileName);
            var safeFileName = $"patients/{request.PatientId}/{Guid.NewGuid()}{fileExtension}";

            // 3. MinIO'ya Yükle
            var imageUrl = await _storageService.UploadAsync(request.FileStream, safeFileName, request.ContentType);

            // 4. --- DEĞİŞEN KISIM BURASI ---
            // Resmi doğrudan Entity olarak oluşturuyoruz.
            var newImage = new TreatmentImage(request.PatientId, imageUrl, request.ImageType);

            // Hasta üzerinden değil, doğrudan Resim tablosuna ekliyoruz.
            // Bu sayede EF Core "Hasta güncellendi mi?" karmaşasına girmez, sadece Insert yapar.
            _context.TreatmentImages.Add(newImage);

            // 5. Kaydet
            await _context.SaveChangesAsync(cancellationToken);

            return request.PatientId;
        }
    }
}