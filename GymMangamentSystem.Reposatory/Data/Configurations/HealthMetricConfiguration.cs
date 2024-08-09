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
            builder.HasKey(h => h.HealthMetricId);
            builder.HasOne(h => h.User)
                   .WithMany(u => u.HealthMetrics)
                   .HasForeignKey(h => h.UserId);
            builder.Property(x => x.Value).HasColumnType("decimal(18,2)");
        }
    }
}
