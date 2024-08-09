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
    public class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.HasMany(u => u.Enrollments)
                   .WithOne(e => e.User)
                   .HasForeignKey(e => e.UserId);

            builder.HasMany(u => u.Attendances)
                   .WithOne(a => a.User)
                   .HasForeignKey(a => a.UserId);

            builder.HasMany(u => u.HealthMetrics)
                   .WithOne(h => h.User)
                   .HasForeignKey(h => h.UserId);

            builder.HasMany(u => u.Feedbacks)
                   .WithOne(f => f.User)
                   .HasForeignKey(f => f.UserId);

            builder.HasMany(u => u.Notifications)
                   .WithOne(n => n.User)
                   .HasForeignKey(n => n.UserId);

            builder.HasMany(u => u.Memberships)
                   .WithOne(m => m.User)
                   .HasForeignKey(m => m.UserId);

            builder.HasOne(u => u.NutritionPlan)
                   .WithMany(n => n.Users)
                   .HasForeignKey(u => u.Id);

            builder.HasOne(u => u.DietRecommendation)
                   .WithMany(d => d.Users)
                   .HasForeignKey(u => u.Id);
            builder.HasOne(u => u.NutritionPlan)
               .WithMany(n => n.Users)
               .HasForeignKey(u => u.NutritionPlanId) 
               .OnDelete(DeleteBehavior.NoAction);    

            builder.HasOne(u => u.DietRecommendation)
                   .WithMany(d => d.Users)
                   .HasForeignKey(u => u.DietRecommendationId) 
                   .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
