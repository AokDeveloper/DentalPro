using DentalApp.Domain.Common;
using DentalApp.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalApp.Domain.Entities
{
    public class Patient: BaseEntity
    {
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string TCKN { get; private set; }
        public string PhoneNumber { get; private set; }
        public DateTime? BirthDate { get; private set; }

        public ICollection<Appointment> Appointments { get; private set; } = new List<Appointment>();
        public ICollection<TreatmentImage> TreatmentImages { get; private set; } = new List<TreatmentImage>();

        protected Patient()
        {
        }

        public Patient(string firstName, string lastName, string tckn, string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(firstName)) throw new ArgumentException("İsim boş olamaz.");

            FirstName = firstName;
            LastName = lastName;
            TCKN = tckn;
            PhoneNumber = phoneNumber;
        }
        // Domain Behaviors (İş Mantığı Metotları)
        public void UpdateContactInfo(string newPhone)
        {
            if (string.IsNullOrWhiteSpace(newPhone)) throw new ArgumentException("Telefon boş olamaz.");
            PhoneNumber = newPhone;
            UpdatedOn = DateTime.UtcNow;
        }

        public void AddImage(string imageUrl, TreatmentImageType type)
        {
            TreatmentImages.Add(new TreatmentImage(this.Id, imageUrl, type));
        }
    }
}
