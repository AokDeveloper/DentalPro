using DentalApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalApp.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<Patient> Patients { get; }
        DbSet<Appointment> Appointments { get; }
        DbSet<TreatmentImage> TreatmentImages { get; }
        DbSet<AuditLog> AuditLogs { get; } 
        DbSet<AccessLog> AccessLogs { get; }
        DbSet<Doctor> Doctors { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
    