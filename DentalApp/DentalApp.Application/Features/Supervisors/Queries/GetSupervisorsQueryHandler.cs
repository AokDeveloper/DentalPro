using DentalApp.Application.Common.Interfaces;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DentalApp.Application.Features.Supervisors.Queries
{
    public class GetSupervisorsQueryHandler : IRequestHandler<GetAllSupervisorsQuery, GetAllSupervisorsResponse>
    {
        private readonly IApplicationDbContext _context;
        public GetSupervisorsQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<GetAllSupervisorsResponse> Handle(GetAllSupervisorsQuery request, CancellationToken cancellationToken)
        {
            var supervisors = await _context.Supervisors
                 .AsNoTracking()
                 .OrderByDescending(p => p.FullName)
                 .ProjectToType<SupervisorDto>() // Büyü burada! MappingConfig'deki kuralı uygular.
                 .ToListAsync(cancellationToken);
            return new GetAllSupervisorsResponse {Supervisors = supervisors};
        }
    }
}
