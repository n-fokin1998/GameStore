﻿using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using GameStore.Domain.Entities;

namespace GameStore.Domain.EF.EntityConfigurations
{
    public class UserEntityConfiguration : EntityTypeConfiguration<User>
    {
        public UserEntityConfiguration()
        {
            Property(p => p.Name).HasMaxLength(450).HasColumnAnnotation(
                "Index",
                new IndexAnnotation(new[] { new IndexAttribute("Index1") { IsUnique = true } }));
            Property(p => p.Email).HasMaxLength(450).HasColumnAnnotation(
                "Index",
                new IndexAnnotation(new[] { new IndexAttribute("Index2") { IsUnique = true } }));
            HasMany(u => u.Roles).WithMany(r => r.Users);
        }
    }
}