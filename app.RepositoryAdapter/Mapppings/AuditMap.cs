using app.Domain.Models.Audit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace app.RepositoryAdapter.Mapppings
{
    public class AuditMap : IEntityTypeConfiguration<Audit>
    {
        public void Configure(EntityTypeBuilder<Audit> builder)
        {
            builder.ToTable("Audit")
                .HasKey(c => c.Id);

            builder.Property(x => x.Id)
                .HasColumnName("Id");
            builder.Property(x => x.DateTime)
                .HasColumnName("DateTime");
            builder.Property(x => x.Type)
                .HasColumnName("Type");
            builder.Property(x => x.User)
                .HasColumnName("User");
            builder.Property(x => x.TableName)
                .HasColumnName("TableName");
            builder.Property(x => x.KeyValues)
                .HasColumnName("KeyValues");
            builder.Property(x => x.OldValues)
                .HasColumnName("OldValues");
            builder.Property(x => x.NewValues)
                .HasColumnName("NewValues");
            builder.Property(x => x.ChangedColumns)
                .HasColumnName("ChangedColumns");

            builder.Ignore(x => x.TypeString);
        }
    }
}