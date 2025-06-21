using back_end.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace back_end.Configurations
{
    public class AttendanceConfiguration : IEntityTypeConfiguration<Attendance>
    {
        public void Configure(EntityTypeBuilder<Attendance> builder)
        {
            builder.HasKey(a => a.Id);

            builder
                .HasOne(a => a.Machine)
                .WithMany(m => m.Attendances)
                .HasForeignKey(a => a.MachineCode);

            builder.Property(a => a.MachineCode)
                .HasDefaultValue(0);

            builder
                .HasOne(a => a.Employee)
                .WithMany(e => e.Attendances)
                .HasForeignKey(a => a.EmployeeId);

            builder.Property(a => a.CheckDate)
                  .IsRequired();

            builder.Property(a => a.Latitude)
                .HasPrecision(9, 6);

            builder.Property(a => a.Longitude)
                .HasPrecision(9, 6);
        }
    }
}
