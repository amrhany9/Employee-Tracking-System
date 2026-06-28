using back_end.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace back_end.Configurations
{
    public class AccountConfiguration : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder
                .HasKey(a => a.Id);

            builder
                .HasOne(a => a.Employee)
                .WithOne(e => e.Account)
                .HasForeignKey<Account>(a => a.EmployeeId);

            builder.Property(a => a.Username)
                  .IsRequired()
                  .HasMaxLength(50);

            builder.Property(a => a.Password)
                  .IsRequired()
                  .HasMaxLength(50);

            builder
                .HasOne(a => a.Role)
                .WithMany(e => e.Accounts)
                .HasForeignKey(a => a.RoleId);
        }
    }
}
