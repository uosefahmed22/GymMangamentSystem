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
            builder.HasIndex(u => u.DisplayName).IsUnique();

            builder.HasMany(u => u.WorkoutPlans)
                .WithOne(wp => wp.Trainer)
                .HasForeignKey(wp => wp.TrainerId)
                .OnDelete(DeleteBehavior.Restrict); 

            builder.HasMany(u => u.NutritionPlans)
                .WithMany(np => np.Users)
                .UsingEntity<Dictionary<string, object>>();

            builder.HasMany(u => u.Attendances)
                .WithOne(a => a.User)
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(u => u.HealthMetrics)
                .WithOne(hm => hm.User)
                .HasForeignKey(hm => hm.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(u => u.Feedbacks)
                .WithOne(f => f.User)
                .HasForeignKey(f => f.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(u => u.Notifications)
                .WithOne(n => n.User)
                .HasForeignKey(n => n.UserId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasMany(u => u.Memberships)
                .WithOne(m => m.User)
                .HasForeignKey(m => m.UserId)
                .OnDelete(DeleteBehavior.Cascade); 

            builder.HasQueryFilter(u => !u.IsDeleted);
        }
    }

}
