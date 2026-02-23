using DentalApp.Application.Common.Interfaces;
using DentalApp.Domain.Entities;
using MediatR;


namespace DentalApp.Application.Features.Doctors.Commands.CreateDoctor
{
    public class CreateDoctorCommandHandler : IRequestHandler<CreateDoctorCommand, Guid>
    {
        private readonly IApplicationDbContext _context;

        public CreateDoctorCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> Handle(CreateDoctorCommand request, CancellationToken cancellationToken)
        {
         var entity = new Doctor(
              request.TCKN,
         request.FirstName,
         request.LastName
        
        
     );
            _context.Doctors.Add(entity);

            // Kaydet
            await _context.SaveChangesAsync(cancellationToken);

            // Yeni oluşan ID'yi döndür
            return entity.Id;

        }
    }
}
