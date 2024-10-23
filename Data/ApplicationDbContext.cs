using back_end.Configurations;
using back_end.Entities;
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
            new UserAccountEntityTypeConfiguration().Configure(modelBuilder.Entity<UserAccount>());
            new UserAttendanceRecordEntityTypeConfiguration().Configure(modelBuilder.Entity<UserAttendanceRecord>());
        }

        public DbSet<User> Users { get; set; }
        public DbSet<UserAccount> UsersAccounts { get; set; }
        public DbSet<UserAttendanceRecord> UsersAttendanceRecords { get; set; }
    }
}
