using DentalApp.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mapster;



namespace DentalApp.Application.Features.Patients.Queries.GetAllPatients
{
    public class GetAllPatientsQueryHandler : IRequestHandler<GetAllPatientsQuery, GetAllPatientsResponse>
    {
        private readonly IApplicationDbContext _context;

        public GetAllPatientsQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<GetAllPatientsResponse> Handle(GetAllPatientsQuery request, CancellationToken cancellationToken)
        {
            // Veritabanından çek ve DTO'ya dönüştür

            var patients = await _context.Patients
            .AsNoTracking()
            .OrderByDescending(p => p.CreatedOn)
            .ProjectToType<PatientDto>() // Büyü burada! MappingConfig'deki kuralı uygular.
            .ToListAsync(cancellationToken);


            return new GetAllPatientsResponse { Patients = patients };
        }
    }
}
