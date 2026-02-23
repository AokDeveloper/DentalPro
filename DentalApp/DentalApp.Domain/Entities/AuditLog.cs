using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalApp.Domain.Entities
{
    public class AuditLog
    {
        public int Id { get; set; }
        public string? UserId { get; set; }      // İşlemi yapan kişi
        public string? Type { get; set; }        // Create, Update, Delete
        public string? TableName { get; set; }   // Hangi tablo? (Patients, Appointments)
        public DateTime DateTime { get; set; }   // Ne zaman?
        public string? OldValues { get; set; }   // Eski hali (JSON)
        public string? NewValues { get; set; }   // Yeni hali (JSON)
        public string? PrimaryKey { get; set; }  // Etkilenen satırın ID'si
    }
}