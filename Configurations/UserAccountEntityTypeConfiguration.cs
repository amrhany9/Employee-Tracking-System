using back_end.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace back_end.Configurations
{
    public class UserAccountEntityTypeConfiguration : IEntityTypeConfiguration<UserAccount>
    {
        public void Configure(EntityTypeBuilder<UserAccount> builder)
        {
            builder
                .HasKey(x => x.UserAccountId);

            builder
                .HasOne(u => u.User)
                .WithOne(ua => ua.UserAccount)
                .HasForeignKey<UserAccount>(u => u.UserId)
                .IsRequired(false);

            builder.Property(x => x.UserName)
                  .IsRequired()
                  .HasMaxLength(50);

            builder.Property(x => x.Password)
                  .IsRequired()
                  .HasMaxLength(255);

            builder.Property(x => x.Role)
                  .IsRequired()
                  .HasMaxLength(10);

            builder.Property(x => x.IsCheckedIn)
                  .HasDefaultValue(false);
        }
    }
}
