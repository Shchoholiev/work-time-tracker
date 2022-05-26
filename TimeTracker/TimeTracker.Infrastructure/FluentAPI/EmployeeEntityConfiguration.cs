using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TimeTracker.Core.Entities;

namespace TimeTracker.Infrastructure.FluentAPI
{
    public class EmployeeEntityConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.HasMany<Record>(e => e.Records)
                   .WithOne(r => r.Employee);
        }
    }
}
