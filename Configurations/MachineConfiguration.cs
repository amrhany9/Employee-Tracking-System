using back_end.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace back_end.Configurations
{
    public class MachineConfiguration : IEntityTypeConfiguration<Machine>
    {
        public void Configure(EntityTypeBuilder<Machine> builder)
        {
            builder
                .HasKey(x => x.machineCode);

            builder.Property(x => x.machineName)
                .HasMaxLength(50);

            builder.Property(x => x.machineIp)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x => x.machinePort)
                .IsRequired();
        }
    }
}
