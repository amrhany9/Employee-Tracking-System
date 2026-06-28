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
            builder.HasKey(a => a.Id);

            builder.Property(a => a.CheckType)
                .IsRequired();

            builder.Property(a => a.CheckDate)
                .IsRequired();

            builder.Property(a => a.Latitude)
                .HasPrecision(9, 6);

            builder.Property(a => a.Longitude)
                .HasPrecision(9, 6);

            builder.Property(a => a.Description)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(a => a.Status)
                .HasDefaultValue(RequestStatus.Pending)
                .IsRequired();

            builder.HasOne(a => a.Employee)
                .WithMany(a => a.AttendanceRequests)
                .HasForeignKey(a => a.EmployeeId);
        }
    }
}
