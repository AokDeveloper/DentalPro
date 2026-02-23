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
    public class PatientConfiguration:IEntityTypeConfiguration<Patient>
    {
        public void Configure(EntityTypeBuilder<Patient> builder)
        {
            // Tablo Adı
            builder.ToTable("Patients");

            // Primary Key
            builder.HasKey(p => p.Id);

            // TCKN Ayarları (Zorunlu, 11 karakter ve Unique Index)
            builder.Property(p => p.TCKN)
                .IsRequired()
                .HasMaxLength(11)
                .IsFixedLength();

            builder.HasIndex(p => p.TCKN).IsUnique(); // Aynı TC ile iki kayıt olamaz!

            // İsim Soyisim Ayarları
            builder.Property(p => p.FirstName).IsRequired().HasMaxLength(50);
            builder.Property(p => p.LastName).IsRequired().HasMaxLength(50);

            // İlişkiler (Bir Hastanın Çok Randevusu Olur)
            builder
                .HasMany(p => p.Appointments)
                .WithOne(a => a.Patient)
                .HasForeignKey(a => a.PatientId)
                .OnDelete(DeleteBehavior.Restrict); // Hasta silinirse randevuları silinmesin, hata versin (Veri güvenliği)
        }
    }
}
