using GymMangamentSystem.Core.Models.Business;
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
                   .HasForeignKey(m => m.UserId);
            builder.Property(x => x.Price).HasColumnType("decimal(18,2)");

        }
    }
}
