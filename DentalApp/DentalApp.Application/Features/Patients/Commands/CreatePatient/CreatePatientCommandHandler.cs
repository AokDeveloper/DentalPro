using DentalApp.Application.Common.Interfaces;
using DentalApp.Domain.Entities;
using MediatR;

namespace DentalApp.Application.Features.Patients.Commands.CreatePatient
{
    public class CreatePatientCommandHandler : IRequestHandler<CreatePatientCommand, Guid>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public CreatePatientCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task<Guid> Handle(CreatePatientCommand request, CancellationToken cancellationToken)
        {

            var currentDoctorId = _currentUserService.DoctorId;


            if (currentDoctorId == null)
            {
                throw new UnauthorizedAccessException("Geçerli bir doktor oturumu bulunamadı. Lütfen tekrar giriş yapın.");
            }


            var entity = new Patient(
                request.FirstName,
                request.LastName,
                request.TCKN,
                request.PhoneNumber,
                request.BirthDate,
                currentDoctorId.Value,
                request.SupervisorId
            );


            // 4. Veritabanına ekle 
            // Not: Yeni ID üretimi veritabanına bağlı değilse (Guid olduğu için biz üretiyoruz), EF Core'da Add() metodu AddAsync()'den daha performanslıdır.
            _context.Patients.Add(entity);


            await _context.SaveChangesAsync(cancellationToken);


            return entity.Id;
        }
    }
}