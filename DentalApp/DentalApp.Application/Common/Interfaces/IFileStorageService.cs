using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalApp.Application.Common.Interfaces
{
    public interface IFileStorageService
    {
        // Dosyayı yükler ve erişim yolunu (URL) döner
        Task<string> UploadAsync(Stream fileStream, string fileName, string contentType);

        // (İsterseniz ileride DeleteAsync vb. ekleyebilirsiniz)
    }
}