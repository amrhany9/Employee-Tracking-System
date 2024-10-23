using back_end.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace back_end.Configurations
{
    public class UserAttendanceRecordEntityTypeConfiguration : IEntityTypeConfiguration<UserAttendanceRecord>
    {
        public void Configure(EntityTypeBuilder<UserAttendanceRecord> builder)
        {
            builder.HasKey(ar => ar.AttendanceId);

            builder.HasOne(ar => ar.User)
                  .WithMany(u => u.AttendanceRecords)
                  .HasForeignKey(ar => ar.UserId);

            builder.Property(ar => ar.CheckType)
                  .IsRequired()
                  .HasMaxLength(10);

            builder.Property(ar => ar.CheckDate)
                  .IsRequired();
        }
    }
}
