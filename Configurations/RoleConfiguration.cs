using back_end.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace back_end.Configurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.HasKey(x => x.roleId);

            builder.Property(x => x.roleNameEn)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x => x.roleNameAr)
                .HasMaxLength(50);
        }
    }
}
