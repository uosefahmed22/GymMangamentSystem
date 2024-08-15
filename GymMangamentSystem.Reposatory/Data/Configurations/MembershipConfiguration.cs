﻿using GymMangamentSystem.Core.Models.Business;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangamentSystem.Reposatory.Data.Configurations
{
    public class MembershipConfiguration : IEntityTypeConfiguration<Membership>
    {
        public void Configure(EntityTypeBuilder<Membership> builder)
        {
            builder.HasKey(m => m.MembershipId);
            builder.HasOne(m => m.User)
                   .WithMany(u => u.Memberships)
                   .HasForeignKey(m => m.UserId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(m => m.Class)
                   .WithMany(c => c.Memberships)
                   .HasForeignKey(m => m.ClassId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(m => m.Payments)
                   .WithOne(p => p.Membership)
                   .HasForeignKey(p => p.MembershipId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.Property(m => m.Price)
                   .HasColumnType("decimal(18,2)");

            builder.HasQueryFilter(u => !u.IsDeleted);
        }
    }

}
