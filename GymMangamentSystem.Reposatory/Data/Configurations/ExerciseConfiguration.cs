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
    public class ExerciseConfiguration : IEntityTypeConfiguration<Exercise>
    {
        public void Configure(EntityTypeBuilder<Exercise> builder)
        {
            builder.HasKey(e => e.ExerciseId);
            builder.HasOne(e => e.WorkoutPlan)
                   .WithMany(wp => wp.Exercises)
                   .HasForeignKey(e => e.WorkoutPlanId)
                   .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(e => e.ExerciseCategory)
                   .WithMany(ec => ec.Exercises)
                   .HasForeignKey(e => e.ExerciseCategoryId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasQueryFilter(u => !u.IsDeleted);
        }
    }
}
