using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TimeTracker.Core.Entities;

namespace TimeTracker.Infrastructure.FluentAPI
{
    public class RecordEntityConfiguration : IEntityTypeConfiguration<Record>
    {
        public void Configure(EntityTypeBuilder<Record> builder)
        {
            builder.HasOne<Project>(r => r.Project)
                   .WithMany(p => p.Records);

            builder.HasOne<Employee>(r => r.Employee)
                   .WithMany(e => e.Records);

            builder.HasOne<ActivityType>(r => r.ActivityType)
                   .WithMany(at => at.Records);

            builder.HasOne<Role>(r => r.Role)
                   .WithMany(r => r.Records);
        }
    }
}
