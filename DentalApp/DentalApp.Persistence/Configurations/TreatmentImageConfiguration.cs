using DentalApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalApp.Persistence.Configurations
{
    public class TreatmentImageConfiguration : IEntityTypeConfiguration<TreatmentImage>
    {
        public void Configure(EntityTypeBuilder<TreatmentImage> builder)
        {
            // 1. Tablo Adı
            builder.ToTable("TreatmentImages");

            // 2. Primary Key
            builder.HasKey(t => t.Id);

            // 3. İlişki Tanımı (En Kritik Kısım)
            // Bir resmin bir hastası vardır, bir hastanın çok resmi vardır.
            builder.HasOne<Patient>()      // Navigation property olmadığı için Tipi veriyoruz
                   .WithMany(p => p.TreatmentImages)
                   .HasForeignKey(t => t.PatientId)
                   .OnDelete(DeleteBehavior.Cascade); // Hasta silinirse resim kayıtları da silinsin
        }
    }
}