using back_end.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace back_end.Configurations
{
    public class AttendanceEntityTypeConfiguration : IEntityTypeConfiguration<Attendance>
    {
        public void Configure(EntityTypeBuilder<Attendance> builder)
        {
            builder.HasKey(ar => ar.Id);

            builder.HasOne(ar => ar.User)
                  .WithMany(u => u.UserAttendances)
                  .HasForeignKey(ar => ar.UserId);

            builder.Property(ar => ar.CheckType)
                  .IsRequired()
                  .HasMaxLength(10);

            builder.Property(ar => ar.CheckDate)
                  .IsRequired();
        }
    }
}
