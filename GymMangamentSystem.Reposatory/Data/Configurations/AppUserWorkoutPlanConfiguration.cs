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
    public class AppUserWorkoutPlanConfiguration : IEntityTypeConfiguration<AppUserWorkoutPlan>
    {
        public void Configure(EntityTypeBuilder<AppUserWorkoutPlan> builder)
        {
            builder.HasKey(uwp => new { uwp.UserId, uwp.WorkoutPlanId });

            builder.HasOne(uwp => uwp.User)
                   .WithMany(u => u.WorkoutPlans)
                   .HasForeignKey(uwp => uwp.UserId)
                   .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(uwp => uwp.WorkoutPlan)
                   .WithMany(wp => wp.MemberWorkoutPlans)
                   .HasForeignKey(uwp => uwp.WorkoutPlanId)
                   .OnDelete(DeleteBehavior.NoAction);
        }
    }



}
