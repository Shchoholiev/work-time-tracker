using Microsoft.EntityFrameworkCore;
using TimeTracker.Infrastructure.FluentAPI;
using TimeTracker.Core.Entities;

namespace TimeTracker.Infrastructure.EF
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext()
        {
        }

        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ProjectEntityConfiguration());
            modelBuilder.ApplyConfiguration(new RecordEntityConfiguration());
            modelBuilder.ApplyConfiguration(new EmployeeEntityConfiguration());
            modelBuilder.ApplyConfiguration(new RoleEntityConfiguration());
            modelBuilder.ApplyConfiguration(new ActivityTypeEntityConfiguration());
        }

        public DbSet<Project> Projects { get; set; }

        public DbSet<Record> Records { get; set; }

        public DbSet<Employee> Employees { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<ActivityType> ActivityTypes { get; set; }
    }
}
