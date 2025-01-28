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
                .HasKey(cs => cs.companyId);

            builder.Property(cs => cs.companyNameEn)
                .HasMaxLength(50);

            builder.Property(cs => cs.companyNameAr)
                .HasMaxLength(50);

            builder.Property(cs => cs.companyLatitude)
                .HasPrecision(9, 6);

            builder.Property(cs => cs.companyLongitude)
                .HasPrecision(9, 6);

            builder
               .HasOne(cs => cs.chairman)
               .WithOne()
               .HasForeignKey<CompanySetup>(cs => cs.chairmanId);
        }
    }
}
