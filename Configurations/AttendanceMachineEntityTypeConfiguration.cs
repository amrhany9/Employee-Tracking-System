using back_end.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace back_end.Configurations
{
    public class AttendanceMachineEntityTypeConfiguration : IEntityTypeConfiguration<AttMachine>
    {
        public void Configure(EntityTypeBuilder<AttMachine> builder)
        {
            builder
                .HasKey(x => x.Id);

            builder.Property(x => x.MachineCode)
                .IsRequired();

            builder.Property(x => x.MachineName)
                .HasMaxLength(50);

            builder.Property(x => x.MachineIP)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x => x.MachinePort)
                .IsRequired();

            builder.Property(x => x.IsDeleted)
                .HasDefaultValue(false);
        }
    }
}
