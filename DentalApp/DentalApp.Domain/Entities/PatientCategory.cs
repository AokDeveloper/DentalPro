using DentalApp.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalApp.Domain.Entities
{
    public class PatientCategory : BaseEntity
    {
        public string Name { get; set; }
     
        public Guid? ParentId { get; set; }
        public PatientCategory Parent { get; set; }
        public ICollection<PatientCategory> SubCategories { get; set; } = new List<PatientCategory>();
      
        public ICollection<Patient> Patients { get; set; } = new List<Patient>();
    }
}
