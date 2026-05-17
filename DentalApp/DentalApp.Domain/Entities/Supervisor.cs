using DentalApp.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalApp.Domain.Entities
{
    public class Supervisor:BaseEntity
    {
        public string FullName { get; set; }
 
        public ICollection<Patient> Patients { get; private set; } = new List<Patient>();
    }
}
