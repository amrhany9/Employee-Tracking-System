using back_end.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace back_end.Configurations
{
    public class AttendanceArchiveEntityTypeConfiguration : IEntityTypeConfiguration<AttendanceArchive>
    {
        public void Configure(EntityTypeBuilder<AttendanceArchive> builder)
        {
            builder.HasKey(ar => ar.Id);

            builder.Property(ar => ar.CheckType)
                  .IsRequired()
                  .HasMaxLength(10);

            builder.Property(ar => ar.CheckDate)
                  .IsRequired();
        }
    }
}
