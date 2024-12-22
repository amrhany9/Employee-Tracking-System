using back_end.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace back_end.Configurations
{
    public class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder
                .HasKey(x => x.Id);

            builder
                .HasOne(x => x.Department)
                .WithMany(x => x.Users)
                .HasForeignKey(x => x.DepartmentId);

            builder.Property(x => x.FullNameEn)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.FullNameAr)
                .IsRequired()
                .IsUnicode(true)
                .UseCollation("Arabic_CI_AS")
                .HasMaxLength(100);

            builder.Property(x => x.Email)
                .HasMaxLength(100);

            builder.Property(x => x.Phone)
                .HasMaxLength(20);

            builder.Property(x => x.UserPhotoPath)
                .HasMaxLength(255);

            builder.Property(x => x.IsDeleted)
                .HasDefaultValue(false);
        }
    }
}
