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
            builder.HasOne(m => m.User);


            builder.HasOne(m => m.Class)
                   .WithMany(c => c.Memberships)
                   .HasForeignKey(m => m.ClassId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.Property(m => m.Price)
                   .HasColumnType("decimal(18,2)");

            builder.HasQueryFilter(u => !u.IsDeleted);
        }
    }

}
