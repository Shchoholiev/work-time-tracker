using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TimeTracker.Core.Entities;

namespace TimeTracker.Infrastructure.FluentAPI
{
    public class ProjectEntityConfiguration : IEntityTypeConfiguration<Project>
    {
        public void Configure(EntityTypeBuilder<Project> builder)
        {
            builder.HasMany<Record>(p => p.Records)
                   .WithOne(r => r.Project);
        }
    }
}
