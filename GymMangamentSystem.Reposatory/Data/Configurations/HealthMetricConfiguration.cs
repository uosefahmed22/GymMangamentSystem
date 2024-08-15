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
    public class HealthMetricConfiguration : IEntityTypeConfiguration<HealthMetric>
    {
        public void Configure(EntityTypeBuilder<HealthMetric> builder)
        {
            builder.HasKey(hm => hm.HealthMetricId);
            builder.HasOne(hm => hm.User)
                   .WithMany(u => u.HealthMetrics)
                   .HasForeignKey(hm => hm.UserId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.Property(hm => hm.Value)
                   .HasColumnType("decimal(18,2)");

            builder.HasQueryFilter(u => !u.IsDeleted);
        }
    }
}
