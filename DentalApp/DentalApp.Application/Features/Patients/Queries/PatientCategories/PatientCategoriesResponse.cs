using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalApp.Application.Features.Patients.Queries.PatientCategories
{
    public record PatientCategoryItemDto(Guid Id, string Name);
       
    public record PatientCategoryGroupDto(Guid GroupId, string GroupName, List<PatientCategoryItemDto> Items);
    public record PatientCategoriesResponse
    {
        public List<PatientCategoryGroupDto> Categories { get; init; } = new ();
    }
}
