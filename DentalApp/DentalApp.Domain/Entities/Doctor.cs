using DentalApp.Domain.Common;

namespace DentalApp.Domain.Entities
{
    public class Doctor : BaseEntity
    {
        public string TCKN { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string? AppUserId { get; set; }
        public AppUser? AppUser { get; set; }
        public ICollection<Appointment> Appointments { get; set; }
        public ICollection<Patient> Patients { get; private set; } = new List<Patient>();

        protected Doctor()
        {
        }
        public Doctor(string tckn, string firstName, string lastName)
        {
            if (string.IsNullOrWhiteSpace(firstName)) throw new ArgumentException("İsim boş olamaz.");

            TCKN = tckn;
            FirstName = firstName;
            LastName = lastName;
            
            
        }
    }

}
