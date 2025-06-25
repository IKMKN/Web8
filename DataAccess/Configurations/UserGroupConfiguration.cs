using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Web8.Models.Entities;
using Web8.Models.Enums;

namespace DataAccess.Configurations;

public class UserGroupConfiguration : IEntityTypeConfiguration<UserGroup>
{
    public void Configure(EntityTypeBuilder<UserGroup> builder)
    {
        builder.HasKey(s => s.UserGroupId);

        builder
            .Property(s => s.UserGroupCode)
            .HasConversion<string>();

        builder.HasData(
            new UserGroup
            {
                UserGroupId = 1,
                UserGroupCode = UserGroupCode.Admin,
                Description = "Administrator"
            },
            new UserGroup
            {
                UserGroupId = 2,
                UserGroupCode = UserGroupCode.User,
                Description = "Average user"
            });
    }
}
