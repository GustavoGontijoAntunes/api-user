using app.Domain.Models;
using app.Domain.Models.Audit;
using app.Domain.Models.Authentication;
using app.Domain.Models.Enum;
using Microsoft.EntityFrameworkCore;

namespace app.RepositoryAdapter
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        { }

        public DbSet<Audit> AuditLogs { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<ProfilePermission> ProfilePermissions { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }

        public virtual async Task<int> SaveChangesAsync(string userId = null)
        {
            if (!string.IsNullOrEmpty(userId))
            {
                OnBeforeSaveChanges(userId);
            }
            var result = await base.SaveChangesAsync();
            return result;
        }

        public virtual int SaveChanges(string userName = null)
        {
            if (!string.IsNullOrEmpty(userName))
            {
                OnBeforeSaveChanges(userName);
            }
            var result = base.SaveChanges();
            return result;
        }

        private void OnBeforeSaveChanges(string userName)
        {
            ChangeTracker.DetectChanges();
            var auditEntries = new List<AuditEntry>();

            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.Entity is Audit || entry.State == EntityState.Detached || entry.State == EntityState.Unchanged)
                    continue;

                var auditEntry = new AuditEntry(entry);
                var entryDatabaseValues = entry.GetDatabaseValues();

                auditEntry.TableName = entry.Entity.GetType().Name;
                auditEntry.UserId = userName;

                foreach (var property in entry.Properties)
                {
                    string propertyName = property.Metadata.Name;

                    if (property.Metadata.IsPrimaryKey())
                    {
                        auditEntry.KeyValues[propertyName] = property.CurrentValue;
                        continue;
                    }

                    if (entry.State != EntityState.Added)
                        property.OriginalValue = entryDatabaseValues.GetValue<object>(propertyName);

                    switch (entry.State)
                    {
                        case EntityState.Added:
                            auditEntry.AuditType = AuditType.Create;
                            auditEntry.NewValues[propertyName] = property.CurrentValue;
                            break;

                        case EntityState.Deleted:
                            auditEntry.AuditType = AuditType.Delete;
                            auditEntry.OldValues[propertyName] = property.OriginalValue;
                            break;

                        case EntityState.Modified:
                            if (property.IsModified 
                                && !(property.CurrentValue == null && property.OriginalValue == null)
                                && ((property.CurrentValue != null && property.OriginalValue == null)
                                 || (property.CurrentValue == null && property.OriginalValue != null)
                                 || !property.CurrentValue.Equals(property.OriginalValue)))
                            {
                                auditEntry.ChangedColumns.Add(propertyName);
                                auditEntry.AuditType = AuditType.Update;
                                auditEntry.OldValues[propertyName] = property.OriginalValue;
                                auditEntry.NewValues[propertyName] = property.CurrentValue;
                            }
                            break;

                        default:
                            continue;
                    }
                }

                if (entry.State == EntityState.Added || entry.State == EntityState.Deleted || (entry.State == EntityState.Modified && auditEntry.ChangedColumns.Count > 0))
                    auditEntries.Add(auditEntry);
            }

            base.SaveChanges();

            foreach (var auditEntry in auditEntries)
            {
                if(auditEntry.AuditType == AuditType.Create)
                {
                    var key = auditEntry.Entry.Properties.Where(x => x.Metadata.IsPrimaryKey()).FirstOrDefault();
                    string propertyName = key.Metadata.Name;
                    auditEntry.KeyValues[propertyName] = key.CurrentValue;                        
                }

                AuditLogs.Add(auditEntry.ToAudit());
            }
        }
    }
}