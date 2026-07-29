using DentalApp.Application.Features.Appointments.Queries.GetAllAppointments;
using DentalApp.Application.Features.Patients.Queries.GetAllPatients;
using DentalApp.Application.Features.Patients.Queries.GetPatientImages;
using DentalApp.Application.Features.Supervisors.Queries;
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
       .ConstructUsing(src => new PatientDto(
           src.Id,
           $"{src.FirstName} {src.LastName}",
           src.TCKN,
           src.PhoneNumber,
           src.BirthDate,
           src.SupervisorId,
           // Supervisor nesnesi doluysa FullName'i al, boşsa "Atanmadı" (veya "") yaz
           src.Supervisor != null ? src.Supervisor.FullName : "Atanmadı"
       ));
            // İsim ve Soyismi birleştirip FullName yap dedik.

            // 2. Resim Listesi Mapping Ayarı
            config.NewConfig<TreatmentImage, TreatmentImageDto>()
                  .Map(dest => dest.ImageUrl, src => src.ImageUrl);
       
            // İsimler aynı olsa bile açıkça belirtmek bazen iyidir (Opsiyonel)
            config.NewConfig<Appointment, AppointmentDto>()
                .Map(dest => dest.PatientName, src => $"{src.Patient.FirstName} {src.Patient.LastName}") // Ad Soyad Birleştirme
                .Map(dest => dest.DoctorName, src => $"{src.Doctor.FirstName} {src.Doctor.LastName}")
                .Map(dest => dest.Status, src => src.Status.ToString()); // Enum'ı string'e çevirme
            config.NewConfig<Supervisor, SupervisorDto>()
        .ConstructUsing(src => new SupervisorDto(
            src.Id,
            src.FullName
              ));
        }
    }
}