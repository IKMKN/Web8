using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Web8.Models.Entities;


namespace DataAccess.Configurations;

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

        var now = DateTime.UtcNow;
        builder.HasData(
            new User
            {
                Id = 1,
                Login = "Admin",
                PasswordHash = "$2a$11$oJPCZ2OPD9Fi5CACy/F01.BBYkIh8lB9nGtOVmUHmvtKf7HdsI.hS",
                CreatedDate = now,
                UserGroupId = 1,
                UserStateId = 1,
            });
    }
}
