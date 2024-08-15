using GymMangamentSystem.Core.Models.Business;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangamentSystem.Reposatory.Data.Configurations
{
    public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.HasKey(p => p.PaymentId);
            builder.HasOne(p => p.Membership)
                   .WithMany(m => m.Payments)
                   .HasForeignKey(p => p.MembershipId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.Property(p => p.Amount)
                   .HasColumnType("decimal(18,2)");

            builder.HasQueryFilter(u => !u.IsDeleted);
        }
    }


}
