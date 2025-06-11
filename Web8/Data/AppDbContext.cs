using Microsoft.EntityFrameworkCore;
using System.Configuration;
using Web8.Data.Configurations;
using Web8.Models.Entities;

namespace Web8.Data;

public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<UserGroup> UsersGroups { get; set; }
    public DbSet<UserState> UsersStates { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new UserStateConfiguration());
        modelBuilder.ApplyConfiguration(new UserGroupConfiguration());

        modelBuilder.Entity<UserGroup>().HasData(
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

        modelBuilder.Entity<UserState>().HasData(
            new UserState
            {
                UserStateId = 1,
                UserStateCode = UserStateCode.Active,
                Description = "Active account"
            },
            new UserState
            {
                UserStateId = 2,
                UserStateCode = UserStateCode.Blocked,
                Description = "Blocked account"
            });

        modelBuilder.Entity<User>().HasData(
            new User
            {
                Id = 1,
                Login = "Admin",
                PasswordHash = "$2a$11$oJPCZ2OPD9Fi5CACy/F01.BBYkIh8lB9nGtOVmUHmvtKf7HdsI.hS",
                CreatedDate = DateTime.SpecifyKind(DateTime.Parse("2020-01-01"), DateTimeKind.Utc),
                UserGroupId = 1,
                UserStateId = 1,
            });
    }
}
