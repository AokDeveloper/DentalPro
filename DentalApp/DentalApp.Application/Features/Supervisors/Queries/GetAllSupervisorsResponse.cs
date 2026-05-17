using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalApp.Application.Features.Supervisors.Queries
{
    public record SupervisorDto(Guid Id, string FullName);

    // API'den dönecek ana cevap
    public record GetAllSupervisorsResponse
    {
        public List<SupervisorDto> Supervisors { get; init; } = new();
    }
}
