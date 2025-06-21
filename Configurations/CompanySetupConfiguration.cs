using back_end.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace back_end.Configurations
{
    public class CompanySetupConfiguration : IEntityTypeConfiguration<CompanySetup>
    {
        public void Configure(EntityTypeBuilder<CompanySetup> builder)
        {
            builder
                .HasKey(cs => cs.Id);

            builder.Property(cs => cs.NameEn)
                .HasMaxLength(50);

            builder.Property(cs => cs.NameAr)
                .HasMaxLength(50);

            builder.Property(cs => cs.Latitude)
                .HasPrecision(9, 6);

            builder.Property(cs => cs.Longitude)
                .HasPrecision(9, 6);

            builder
               .HasOne(cs => cs.chairman)
               .WithOne()
               .HasForeignKey<CompanySetup>(cs => cs.ChairmanId);
        }
    }
}
