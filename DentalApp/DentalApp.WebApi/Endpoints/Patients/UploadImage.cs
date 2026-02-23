using DentalApp.Application.Features.Patients.Commands.UploadImage;
using DentalApp.Domain.Enums;
using MediatR;
using FastEndpoints;


namespace DentalApp.WebApi.Endpoints.Patients
{
    // İstemciden (Frontend) gelecek istek modeli
    public class UploadImageRequest
    {
        public Guid PatientId { get; set; }
        public IFormFile File { get; set; } // Dosya buraya gelecek
        public TreatmentImageType Type { get; set; }
    }

    public class UploadImage : Endpoint<UploadImageRequest, Guid>
    {
        private readonly ISender _sender;

        public UploadImage(ISender sender)
        {
            _sender = sender;
        }

        public override void Configure()
        {
            Post("/api/patients/images");
            AllowFileUploads(); // <-- DİKKAT: Dosya yüklemeye izin veriyoruz
        }

        public override async Task HandleAsync(UploadImageRequest req, CancellationToken ct)
        {

            if (req.File is null)
            {
                ThrowError("Lütfen yüklenecek bir resim dosyası seçiniz!");
                return;
            }

            using var stream = req.File.OpenReadStream();

            var command = new UploadTreatmentImageCommand
            {
                PatientId = req.PatientId,
                FileStream = stream,
                FileName = req.File.FileName,
                ContentType = req.File.ContentType,
                ImageType = req.Type
            };

            var result = await _sender.Send(command, ct);

            await SendOkAsync(result, ct);
        }
    }
}