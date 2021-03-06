﻿using Indice.AspNetCore.Identity.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Indice.AspNetCore.Identity.Data.Mappings
{
    /// <summary>
    /// Entity Framework mapping for type <see cref="User"/>.
    /// </summary>
    /// <typeparam name="TUser">The type of user.</typeparam>
    internal class UserMap<TUser> : IEntityTypeConfiguration<TUser> where TUser : User
    {
        /// <summary>
        /// Configure Entity Framework mapping for type <see cref="User"/>.
        /// </summary>
        /// <param name="entityBuilder"></param>
        public void Configure(EntityTypeBuilder<TUser> entityBuilder) {
            // Configure table name and schema.
            entityBuilder.ToTable(nameof(User), "auth");
            // Configure relationships.
            entityBuilder.HasMany(x => x.Claims).WithOne().HasForeignKey(x => x.UserId);
            entityBuilder.HasMany(x => x.Logins).WithOne().HasForeignKey(x => x.UserId);
            entityBuilder.HasMany(x => x.Roles).WithOne().HasForeignKey(x => x.UserId);
        }
    }
}
