using back_end.Configurations;
using back_end.Models;
using Microsoft.EntityFrameworkCore;

namespace back_end.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            new UserEntityTypeConfiguration().Configure(modelBuilder.Entity<User>());
            new AccountEntityTypeConfiguration().Configure(modelBuilder.Entity<Account>());
            new AttendanceEntityTypeConfiguration().Configure(modelBuilder.Entity<Attendance>());
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
    }
}
