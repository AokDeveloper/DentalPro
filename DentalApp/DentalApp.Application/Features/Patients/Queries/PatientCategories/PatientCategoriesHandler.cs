using DentalApp.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalApp.Application.Features.Patients.Queries.PatientCategories
{
    public class PatientCategoriesHandler : IRequestHandler<PatientCategoriesQuery, PatientCategoriesResponse>
    {
        private readonly IApplicationDbContext _context;

        public PatientCategoriesHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PatientCategoriesResponse> Handle(PatientCategoriesQuery request, CancellationToken cancellationToken)
        {
            // Tüm kategorileri AsNoTracking ile hızlıca belleğe alıyoruz
            var allCategories = await _context.PatientCategories
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            // RAM üzerinde Angular'ın istediği hiyerarşiyi (Group -> Items) kuruyoruz
            var groupedCategories = allCategories
                .Where(c => c.ParentId == null) // Sadece Ana Kategoriler
                .Select(parent => new PatientCategoryGroupDto(
                    parent.Id,
                    parent.Name,
                    allCategories
                        .Where(child => child.ParentId == parent.Id) // Ana kategoriye ait alt kategoriler
                        .Select(child => new PatientCategoryItemDto(child.Id, child.Name))
                        .ToList()
                ))
                .ToList();

            // Senin Response formatına uygun olarak listeyi içine koyup dönüyoruz
            return new PatientCategoriesResponse
            {
                Categories = groupedCategories
            };
        }
    }
}