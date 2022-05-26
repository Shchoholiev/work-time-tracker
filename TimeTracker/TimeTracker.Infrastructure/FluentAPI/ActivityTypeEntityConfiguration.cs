using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TimeTracker.Core.Entities;

namespace TimeTracker.Infrastructure.FluentAPI
{
    public class ActivityTypeEntityConfiguration : IEntityTypeConfiguration<ActivityType>
    {
        public void Configure(EntityTypeBuilder<ActivityType> builder)
        {
            builder.HasMany<Record>(at => at.Records)
                   .WithOne(r => r.ActivityType);
        }
    }
}
