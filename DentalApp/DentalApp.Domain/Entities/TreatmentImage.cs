using DentalApp.Domain.Common;
using DentalApp.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalApp.Domain.Entities
{
    public class TreatmentImage:BaseEntity
    {
        public Guid PatientId { get; private set; }
        public string ImageUrl { get; private set; } // MinIO'daki path
        public TreatmentImageType Type { get; private set; }

        protected TreatmentImage() { }
        public TreatmentImage(Guid patientId, string imageUrl, TreatmentImageType type)
        {
            PatientId = patientId;
            ImageUrl = imageUrl;
            Type = type;
        }
    }
}
