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
                .HasKey(x => x.Id);

            builder.Property(x => x.FullNameEn)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x => x.FullNameAr)
                .IsUnicode(true)
                .UseCollation("Arabic_CI_AS")
                .HasMaxLength(50);

            builder
                .HasOne(e => e.Department)
                .WithMany(d => d.Employees)
                .HasForeignKey(e => e.DepartmentId);

            builder.Property(x => x.Email)
                .HasMaxLength(50);

            builder.Property(x => x.Phone)
                .HasMaxLength(20);

            builder.Property(x => x.UserPhotoPath)
                .HasMaxLength(255);

            builder.Property(x => x.IsCheckedIn)
                  .HasDefaultValue(false);

            builder.Property(x => x.IsActive)
                  .HasDefaultValue(true);
        }
    }
}
