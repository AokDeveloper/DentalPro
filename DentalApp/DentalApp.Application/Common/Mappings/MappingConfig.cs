using DentalApp.Application.Features.Appointments.Queries;
using DentalApp.Application.Features.Patients.Queries.GetAllPatients;
using DentalApp.Application.Features.Patients.Queries.GetPatientImages;
using DentalApp.Domain.Entities;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalApp.Application.Common.Mappings
{
    public class MappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            // 1. Hasta Listesi Mapping Ayarı
            config.NewConfig<Patient, PatientDto>()
                .Map(dest => dest.FullName, src => $"{src.FirstName} {src.LastName}");
            // İsim ve Soyismi birleştirip FullName yap dedik.

            // 2. Resim Listesi Mapping Ayarı
            config.NewConfig<TreatmentImage, TreatmentImageDto>()
                .Map(dest => dest.ImageUrl, src => src.ImageUrl);
            // İsimler aynı olsa bile açıkça belirtmek bazen iyidir (Opsiyonel)
            config.NewConfig<Appointment, AppointmentDto>()
                .Map(dest => dest.PatientName, src => $"{src.Patient.FirstName} {src.Patient.LastName}") // Ad Soyad Birleştirme
                .Map(dest => dest.DoctorName, src => $"{src.Doctor.FirstName} {src.Doctor.LastName}")
                .Map(dest => dest.Status, src => src.Status.ToString()); // Enum'ı string'e çevirme
                 
        }
    }
}