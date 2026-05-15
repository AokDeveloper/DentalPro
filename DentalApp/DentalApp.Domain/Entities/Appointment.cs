using DentalApp.Domain.Common;
using DentalApp.Domain.DomainExceptionHandler;
using DentalApp.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalApp.Domain.Entities
{
    public class Appointment: BaseEntity
    {
        public Guid PatientId { get; private set; }
        public DateTime Date { get; private set; }
        public int Duration { get; set; } = 30;
        public AppointmentStatus Status { get; private set; }
        public string? Notes { get; private set; }
        public Guid DoctorId { get; set; } // Foreign Key
        public string? CompletionNotes { get; private set; }



        // EF Core için Navigation Property
        public Doctor Doctor { get; set; }
        public Patient Patient { get; set; } = null!;

        // Constructor

        protected Appointment()
        {

        }


        public Appointment(Guid patientId, DateTime date, string? notes, Guid doctorId)
        {
            if (date < DateTime.UtcNow) throw new ArgumentException("Geçmişe randevu verilemez.");

            PatientId = patientId;
            Date = date;
            Notes = notes;
            Status = AppointmentStatus.Planlandı;
            DoctorId = doctorId;
        }
        // Randevu İptal Mantığı
        public void Cancel(string notes)
        {
            if (Status == AppointmentStatus.Tamamlandı /*|| Status == AppointmentStatus.Cancelled*/)
                throw new InvalidActionException("Tamamlanmış randevu iptal edilemez.");

            Status = AppointmentStatus.Iptal;
            CompletionNotes = notes; //iptal nedeni
            
        }

        // Randevu Tamamlama Mantığı
        public void Complete(string notes)
        {
            if (Status == AppointmentStatus.Iptal)
                throw new InvalidActionException("İptal edilmiş randevu tamamlanamaz.");
            Status = AppointmentStatus.Tamamlandı;
            if(!string.IsNullOrEmpty(notes))
                CompletionNotes = notes;
            
        }
        public void DidntCome(string notes)
        {
            if (Status == AppointmentStatus.Iptal )
                throw new InvalidActionException("İptal edilmiş randevu gelmedi olarak seçilemez.");
            //if (Status == AppointmentStatus.Tamamlandı)
            //    throw new InvalidActionException("Tamamlanmış randevu gelmedi olarak seçilemez.");
            Status = AppointmentStatus.Gelmedi;
            if (!string.IsNullOrEmpty(notes))
                CompletionNotes = notes;

        }
    }
}
