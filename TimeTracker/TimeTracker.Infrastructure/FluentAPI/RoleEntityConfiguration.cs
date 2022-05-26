using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TimeTracker.Core.Entities;

namespace TimeTracker.Infrastructure.FluentAPI
{
    public class RoleEntityConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.HasMany<Record>(r => r.Records)
                   .WithOne(r => r.Role);
        }
    }
}
