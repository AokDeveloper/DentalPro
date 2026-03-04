using DentalApp.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalApp.Application.Features.Patients.Queries.GetPatientImages
{

    public record TreatmentImageDto(Guid Id, string ImageUrl, TreatmentImageType Type, DateTime CreatedOn, DateOnly RecordDate, string? Notes);

    public record GetPatientImagesResponse
    {
        public List<TreatmentImageDto> Images { get; init; } = new();
    }
}

