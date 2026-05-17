using DentalApp.Application.Common.Interfaces;
using DentalApp.Domain.Common;
using DentalApp.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DentalApp.Persistence.Contexts
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>, IApplicationDbContext
    {
        private readonly ICurrentUserService _currentUserService;

        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options,
            ICurrentUserService currentUserService) : base(options)
        {
            _currentUserService = currentUserService;
        }

        public DbSet<Patient> Patients { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<TreatmentImage> TreatmentImages { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }
        public DbSet<AccessLog> AccessLogs { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Supervisor> Supervisors { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }

        // --- HEM TIME STAMP HEM AUDIT LOG BURADA ---
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            // 1. ADIM: Otomatik Tarih Atama (CreatedOn / UpdatedOn)
            // Bu kısım Audit'ten ÖNCE olmalı ki, loglarda tarihi de görelim.
            foreach (var entry in ChangeTracker.Entries<BaseEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedOn = DateTime.UtcNow;
                        break;
                    case EntityState.Modified:
                        entry.Entity.UpdatedOn = DateTime.UtcNow;
                        break;
                }
            }

            // 2. ADIM: Audit Log Hazırlığı
            var auditEntries = OnBeforeSaveChanges(_currentUserService.UserId);

            // 3. ADIM: Kaydetme (Transaction)
            var result = await base.SaveChangesAsync(cancellationToken);

            // 4. ADIM: Audit Log Tamamlama (ID'ler oluştuktan sonra)
            await OnAfterSaveChanges(auditEntries, cancellationToken);

            return result;
        }

        // --- YARDIMCI AUDIT METOTLARI (Aynen Kalıyor) ---

        private List<AuditEntry> OnBeforeSaveChanges(string? userId)
        {
            ChangeTracker.DetectChanges();
            var auditEntries = new List<AuditEntry>();

            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.Entity is AuditLog || entry.State == EntityState.Detached || entry.State == EntityState.Unchanged)
                    continue;

                var auditEntry = new AuditEntry(entry);
                auditEntry.TableName = entry.Entity.GetType().Name;
                auditEntry.UserId = userId;
                auditEntries.Add(auditEntry);

                foreach (var property in entry.Properties)
                {
                    if (property.IsTemporary)
                    {
                        auditEntry.TemporaryProperties.Add(property);
                        continue;
                    }

                    string propertyName = property.Metadata.Name;
                    if (property.Metadata.IsPrimaryKey())
                    {
                        auditEntry.KeyValues[propertyName] = property.CurrentValue;
                        continue;
                    }

                    switch (entry.State)
                    {
                        case EntityState.Added:
                            auditEntry.AuditType = "Create";
                            auditEntry.NewValues[propertyName] = property.CurrentValue;
                            break;

                        case EntityState.Deleted:
                            auditEntry.AuditType = "Delete";
                            auditEntry.OldValues[propertyName] = property.OriginalValue;
                            break;

                        case EntityState.Modified:
                            if (property.IsModified)
                            {
                                auditEntry.AuditType = "Update";
                                auditEntry.OldValues[propertyName] = property.OriginalValue;
                                auditEntry.NewValues[propertyName] = property.CurrentValue;
                            }
                            break;
                    }
                }
            }
            foreach (var auditEntry in auditEntries.Where(e => !e.HasTemporaryProperties))
            {
                AuditLogs.Add(auditEntry.ToAuditLog());
            }
            return auditEntries.Where(e => e.HasTemporaryProperties).ToList();
        }

        private Task OnAfterSaveChanges(List<AuditEntry> auditEntries, CancellationToken cancellationToken)
        {
            if (auditEntries == null || auditEntries.Count == 0)
                return Task.CompletedTask;

            foreach (var auditEntry in auditEntries)
            {
                foreach (var prop in auditEntry.TemporaryProperties)
                {
                    if (prop.Metadata.IsPrimaryKey())
                    {
                        auditEntry.KeyValues[prop.Metadata.Name] = prop.CurrentValue;
                    }
                    else
                    {
                        auditEntry.NewValues[prop.Metadata.Name] = prop.CurrentValue;
                    }
                }
                AuditLogs.Add(auditEntry.ToAuditLog());
            }
            return base.SaveChangesAsync(cancellationToken);
        }
    }

    // AuditEntry sınıfı dosyanın altında aynen durmalı...
    public class AuditEntry
    {
        public AuditEntry(EntityEntry entry)
        {
            Entry = entry;
        }
        public EntityEntry Entry { get; }
        public string? UserId { get; set; }
        public string? TableName { get; set; }
        public Dictionary<string, object> KeyValues { get; } = new();
        public Dictionary<string, object> OldValues { get; } = new();
        public Dictionary<string, object> NewValues { get; } = new();
        public List<PropertyEntry> TemporaryProperties { get; } = new();
        public string AuditType { get; set; }
        public bool HasTemporaryProperties => TemporaryProperties.Any();

        public AuditLog ToAuditLog()
        {
            return new AuditLog
            {
                UserId = UserId,
                Type = AuditType,
                TableName = TableName,
                DateTime = DateTime.UtcNow,
                PrimaryKey = JsonSerializer.Serialize(KeyValues),
                OldValues = OldValues.Count == 0 ? null : JsonSerializer.Serialize(OldValues),
                NewValues = NewValues.Count == 0 ? null : JsonSerializer.Serialize(NewValues)
            };
        }
    }
}