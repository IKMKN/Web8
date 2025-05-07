using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Web8.Models.Entities;

namespace Web8.Data.Configurations;

public class UserGroupConfiguration : IEntityTypeConfiguration<UserGroup>
{
    public void Configure(EntityTypeBuilder<UserGroup> builder)
    {
        builder.HasKey(s => s.UserGroupId);

        builder
            .Property(s => s.UserGroupCode)
            .HasConversion<string>();
    }
}
