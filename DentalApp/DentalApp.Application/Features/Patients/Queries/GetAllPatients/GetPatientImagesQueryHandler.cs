using DentalApp.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mapster;



namespace DentalApp.Application.Features.Patients.Queries.GetAllPatients
{
    public class GetAllPatientsQueryHandler : IRequestHandler<GetAllPatientsQuery, GetAllPatientsResponse>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public GetAllPatientsQueryHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task<GetAllPatientsResponse> Handle(GetAllPatientsQuery request, CancellationToken cancellationToken)
        {

            var currentDoctorId = _currentUserService.DoctorId;
            if (currentDoctorId == null)
            {
                // Güvenlik: Eğer token yoksa veya bozuksa boş liste dön veya hata fırlat
                throw new UnauthorizedAccessException("Hastaları listelemek için geçerli bir doktor oturumu bulunamadı.");
            }
            // Veritabanından çek ve DTO'ya dönüştür

            var patients = await _context.Patients
            .AsNoTracking()
            .Include(a => a.Supervisor)
            .Where(p => p.DoctorId == currentDoctorId.Value)
            .OrderByDescending(p => p.CreatedOn)
            .ProjectToType<PatientDto>() // Büyü burada! MappingConfig'deki kuralı uygular.
            .ToListAsync(cancellationToken);


            return new GetAllPatientsResponse { Patients = patients };
        }
    }
}
