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
                .HasKey(x => x.Code);

            builder.Property(x => x.Name)
                .HasMaxLength(50);

            builder.Property(x => x.Ip)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x => x.Port)
                .IsRequired();
        }
    }
}
