using back_end.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace back_end.Configurations
{
    public class AccountEntityTypeConfiguration : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder
                .HasKey(x => x.Id);

            builder
                .HasOne(u => u.User)
                .WithOne(ua => ua.UserAccount)
                .HasForeignKey<Account>(u => u.UserId)
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
