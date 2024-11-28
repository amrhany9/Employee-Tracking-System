using back_end.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace back_end.Configurations
{
    public class AttendanceArchiveEntityTypeConfiguration : IEntityTypeConfiguration<AttArchive>
    {
        public void Configure(EntityTypeBuilder<AttArchive> builder)
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
