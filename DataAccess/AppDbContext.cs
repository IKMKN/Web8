using Microsoft.EntityFrameworkCore;
using DataAccess.Configurations;
using Domain.Entities;

namespace DataAccess;

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
    }
}
