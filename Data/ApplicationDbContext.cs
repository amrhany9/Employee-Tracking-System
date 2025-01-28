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
            new EmployeeConfiguration().Configure(modelBuilder.Entity<Employee>());
            new DepartmentConfiguration().Configure(modelBuilder.Entity<Department>());
            new AccountConfiguration().Configure(modelBuilder.Entity<Account>());
            new AttendanceConfiguration().Configure(modelBuilder.Entity<Attendance>());
            new AttendanceRequestConfiguration().Configure(modelBuilder.Entity<AttendanceRequest>());
            new MachineConfiguration().Configure(modelBuilder.Entity<Machine>());
            new RoleConfiguration().Configure(modelBuilder.Entity<Role>());
            new CompanySetupConfiguration().Configure(modelBuilder.Entity<CompanySetup>());
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<AttendanceRequest> AttendanceRequests { get; set; }
        public DbSet<Machine> Machines { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<CompanySetup> CompanySetup { get; set; }
    }
}
