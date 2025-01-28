using back_end.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace back_end.Configurations
{
    public class AttendanceConfiguration : IEntityTypeConfiguration<Attendance>
    {
        public void Configure(EntityTypeBuilder<Attendance> builder)
        {
            builder.HasKey(a => a.attendanceId);

            builder
                .HasOne(a => a.machine)
                .WithMany(m => m.attendances)
                .HasForeignKey(a => a.machineCode);

            builder.Property(a => a.machineCode)
                .HasDefaultValue(0);

            builder
                .HasOne(a => a.employee)
                .WithMany(e => e.attendances)
                .HasForeignKey(a => a.employeeId);

            builder.Property(a => a.checkDate)
                  .IsRequired();

            builder.Property(a => a.latitude)
                .HasPrecision(9, 6);

            builder.Property(a => a.longitude)
                .HasPrecision(9, 6);
        }
    }
}
