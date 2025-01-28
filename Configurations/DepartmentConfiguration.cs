using back_end.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace back_end.Configurations
{
    public class DepartmentConfiguration : IEntityTypeConfiguration<Department>
    {
        public void Configure(EntityTypeBuilder<Department> builder)
        {
            builder
                .HasKey(x => x.departmentId);

            builder.Property(x => x.departmentNameEn)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x => x.departmentNameAr)
                .IsUnicode(true)
                .UseCollation("Arabic_CI_AS")
                .HasMaxLength(50);
        }
    }
}
