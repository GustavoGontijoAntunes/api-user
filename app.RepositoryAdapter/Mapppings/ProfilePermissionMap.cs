using app.Domain.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace app.RepositoryAdapter.Mapppings
{
    public class PermissionProfileMap : IEntityTypeConfiguration<ProfilePermission>
    {
        public void Configure(EntityTypeBuilder<ProfilePermission> builder)
        {
            builder.ToTable("profilePermission");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .HasColumnName("Id")
                .ValueGeneratedOnAdd();
        }
    }
}