using DentalApp.Application.Common.Interfaces;
using DentalApp.Domain.Enums;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalApp.Application.Features.Patients.Queries.GetPatientImages
{
    
    public record GetPatientImagesQuery(Guid PatientId) : IRequest<GetPatientImagesResponse>;

    public class GetPatientImagesQueryHandler : IRequestHandler<GetPatientImagesQuery, GetPatientImagesResponse>
    {
        private readonly IApplicationDbContext _context;

        public GetPatientImagesQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<GetPatientImagesResponse> Handle(GetPatientImagesQuery request, CancellationToken cancellationToken)
        {

            var images = await _context.TreatmentImages
            .AsNoTracking()
            .Where(x => x.PatientId == request.PatientId)
            .OrderByDescending(x => x.CreatedOn)
            .ProjectToType<TreatmentImageDto>() // Otomatik mapler
            .ToListAsync(cancellationToken);

            return new GetPatientImagesResponse { Images = images };
        }
    }
}