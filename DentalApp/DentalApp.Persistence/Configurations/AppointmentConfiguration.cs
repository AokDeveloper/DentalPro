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
    public class AppointmentConfiguration : IEntityTypeConfiguration<Appointment>
    {
        public void Configure(EntityTypeBuilder<Appointment> builder)
        {
            // Tablo Adı
            builder.ToTable("Appointments");

            // Primary Key
            builder.HasKey(x => x.Id);

            // İlişkiler
            builder.HasOne(x => x.Patient)
                .WithMany(p => p.Appointments)
                .HasForeignKey(x => x.PatientId)
                .OnDelete(DeleteBehavior.Restrict); // Hasta silinirse geçmiş randevuları silinmesin

            // Alan Ayarları
            builder.Property(x => x.Notes)
                .HasMaxLength(500);

            builder.Property(x => x.Date)
                .IsRequired();
        }
    }
}