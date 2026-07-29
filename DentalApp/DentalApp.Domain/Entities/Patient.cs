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
    public class Patient : BaseEntity
    {
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string TCKN { get; private set; }
        public string PhoneNumber { get; private set; }
        public DateOnly BirthDate { get; private set; }
        public Guid? DoctorId { get; set; }
        public Guid? SupervisorId { get; set; }

        public Doctor? Doctor { get; set; }
        public Supervisor? Supervisor { get; set; }

        public ICollection<Appointment> Appointments { get; private set; } = new List<Appointment>();
        public ICollection<TreatmentImage> TreatmentImages { get; private set; } = new List<TreatmentImage>();

        protected Patient()
        {
        }

        public Patient(string firstName, string lastName, string tckn, string phoneNumber, DateOnly birthDate, Guid doctorId, Guid supervisorId)
        {
            if (string.IsNullOrWhiteSpace(firstName)) throw new InvalidActionException("İsim boş olamaz.");

            FirstName = firstName;
            LastName = lastName;
            TCKN = tckn;
            PhoneNumber = phoneNumber;
            BirthDate = birthDate;
            DoctorId = doctorId;
            SupervisorId = supervisorId;
        }
        // Domain Behaviors (İş Mantığı Metotları)

        public void MarkAsDeleted()
        {
            if (IsDeleted)
            {
                throw new InvalidActionException("Hasta zaten silinmiş.");
            }
            IsDeleted = true;
            UpdatedOn = DateTime.UtcNow;
        }
        public void UpdateContactInfo(string newPhone)
        {
            if (string.IsNullOrWhiteSpace(newPhone)) throw new InvalidActionException("Telefon boş olamaz.");
            PhoneNumber = newPhone;
            UpdatedOn = DateTime.UtcNow;
        }

        public void AddImage(string imageUrl, TreatmentImageType type, DateOnly recordDate, string notes)
        {
            TreatmentImages.Add(new TreatmentImage(this.Id, imageUrl, type, recordDate, notes));
        }

    }
}
