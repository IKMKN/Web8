﻿using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations;

public class UserStateConfiguration : IEntityTypeConfiguration<UserState>
{
    public void Configure(EntityTypeBuilder<UserState> builder)
    {
        builder.HasKey(s => s.UserStateId);

        builder
            .Property(s => s.UserStateCode)
            .HasConversion<string>();


        builder.HasData(
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
    }
}
