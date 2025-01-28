using back_end.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace back_end.Configurations
{
    public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder
                .HasKey(x => x.employeeId);

            builder.Property(x => x.fullNameEn)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x => x.fullNameAr)
                .IsUnicode(true)
                .UseCollation("Arabic_CI_AS")
                .HasMaxLength(50);

            builder
                .HasOne(e => e.department)
                .WithMany(d => d.employees)
                .HasForeignKey(e => e.departmentId);

            builder.Property(x => x.email)
                .HasMaxLength(50);

            builder.Property(x => x.phone)
                .HasMaxLength(20);

            builder.Property(x => x.userPhotoPath)
                .HasMaxLength(255);

            builder.Property(x => x.isCheckedIn)
                  .HasDefaultValue(false);

            builder.Property(x => x.isActive)
                  .HasDefaultValue(true);
        }
    }
}
