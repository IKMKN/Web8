using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Web8.Models.Entities;

namespace Web8.Data.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(x => x.Id);

        builder
            .HasIndex(u => u.Login)
            .IsUnique();

        builder
            .HasOne(u => u.UserGroup)
            .WithMany(g => g.Users)
            .HasForeignKey(u => u.UserGroupId);

        builder
            .HasOne(u => u.UserState)
            .WithMany(s => s.Users)
            .HasForeignKey(u => u.UserStateId);
    }
}
