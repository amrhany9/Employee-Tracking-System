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
                .HasKey(a => a.accountId);

            builder
                .HasOne(a => a.employee)
                .WithOne(e => e.account)
                .HasForeignKey<Account>(a => a.employeeId);

            builder.Property(a => a.username)
                  .IsRequired()
                  .HasMaxLength(50);

            builder.Property(a => a.password)
                  .IsRequired()
                  .HasMaxLength(50);

            builder
                .HasOne(a => a.role)
                .WithMany(e => e.accounts)
                .HasForeignKey(a => a.roleId);
        }
    }
}
