using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Web8.Models.Entities;

namespace Web8.Data.Configurations;

public class UserStateConfiguration : IEntityTypeConfiguration<UserState>
{
    public void Configure(EntityTypeBuilder<UserState> builder)
    {
        builder.HasKey(s => s.UserStateId);

        builder
            .Property(s => s.UserStateCode)
            .HasConversion<string>();

    }
}
