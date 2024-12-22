using back_end.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace back_end.Configurations
{
    public class DepartmentEntityTypeConfiguration : IEntityTypeConfiguration<Department>
    {
        public void Configure(EntityTypeBuilder<Department> builder)
        {
            builder
                .HasKey(x => x.Id);

            builder.Property(x => x.NameEn)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.NameAr)
                .IsRequired()
                .IsUnicode(true)
                .UseCollation("Arabic_CI_AS")
                .HasMaxLength(100);

            builder.Property(x => x.IsDeleted)
                .HasDefaultValue(false);
        }
    }
}
