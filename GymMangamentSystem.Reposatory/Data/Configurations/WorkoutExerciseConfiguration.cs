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
    public class WorkoutExerciseConfiguration : IEntityTypeConfiguration<WorkoutExercise>
    {
        public void Configure(EntityTypeBuilder<WorkoutExercise> builder)
        {
            builder.HasKey(w => w.WorkoutExerciseId);
            builder.HasOne(w => w.Exercise)
                   .WithMany(e => e.WorkoutExercises)
                   .HasForeignKey(w => w.ExerciseId);
            builder.HasOne(w => w.WorkoutPlan)
                   .WithMany(wp => wp.WorkoutExercises)
                   .HasForeignKey(w => w.WorkoutPlanId);
        }
    }

}
