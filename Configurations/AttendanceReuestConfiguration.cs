using back_end.Constants.Enums;
using back_end.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace back_end.Configurations
{
    public class AttendanceRequestConfiguration : IEntityTypeConfiguration<AttendanceRequest>
    {
        public void Configure(EntityTypeBuilder<AttendanceRequest> builder)
        {
            builder.HasKey(a => a.requestId);

            builder.Property(a => a.checkType)
                .IsRequired();

            builder.Property(a => a.checkDate)
                .IsRequired();

            builder.Property(a => a.latitude)
                .HasPrecision(9, 6);

            builder.Property(a => a.longitude)
                .HasPrecision(9, 6);

            builder.Property(a => a.description)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(a => a.status)
                .HasDefaultValue(RequestStatus.Pending)
                .IsRequired();

            builder.HasOne(a => a.employee)
                .WithMany(a => a.attendanceRequests)
                .HasForeignKey(a => a.employeeId);
        }
    }
}
