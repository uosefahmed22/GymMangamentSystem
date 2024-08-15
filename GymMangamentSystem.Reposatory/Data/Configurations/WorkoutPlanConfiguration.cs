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
    public class WorkoutPlanConfiguration : IEntityTypeConfiguration<WorkoutPlan>
    {
        public void Configure(EntityTypeBuilder<WorkoutPlan> builder)
        {
            builder.HasKey(wp => wp.WorkoutPlanId);
            builder.HasOne(wp => wp.Trainer)
                   .WithMany(u => u.WorkoutPlans)
                   .HasForeignKey(wp => wp.TrainerId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(wp => wp.Exercises)
                   .WithOne(e => e.WorkoutPlan)
                   .HasForeignKey(e => e.WorkoutPlanId)
                   .OnDelete(DeleteBehavior.SetNull);

            builder.HasQueryFilter(u => !u.IsDeleted);
        }
    }


}
