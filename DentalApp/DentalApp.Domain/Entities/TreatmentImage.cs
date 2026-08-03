using DentalApp.Domain.Common;
using DentalApp.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalApp.Domain.Entities
{
    public class TreatmentImage : BaseEntity
    {
        public Guid PatientId { get; private set; }
        public string ImageUrl { get; private set; } // MinIO'daki path
        public TreatmentImageType Type { get; private set; }
        public DateOnly RecordDate { get; private set; }
        public string? Notes { get; private set; }
       // public Patient Patient { get; private set; }
        protected TreatmentImage() { }
        public TreatmentImage(Guid patientId, string imageUrl, TreatmentImageType type, DateOnly recordDate, string? notes=null)
        {
            PatientId = patientId;
            ImageUrl = imageUrl;
            Type = type;
            RecordDate = recordDate;
            Notes = notes;
        }
        public void UpdateDetails(DateOnly newDate, string? newNotes)
        {
            RecordDate = newDate;
            Notes = newNotes;
            UpdatedOn = DateTime.UtcNow; // BaseEntity'den geliyorsa
        }
    }
}
