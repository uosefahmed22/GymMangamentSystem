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

    public class MealConfiguration : IEntityTypeConfiguration<Meal>
    {
        public void Configure(EntityTypeBuilder<Meal> builder)
        {
            builder.HasKey(m => m.MealId);
            builder.HasOne(m => m.NutritionPlan)
                   .WithMany(np => np.Meals)
                   .HasForeignKey(m => m.NutritionPlanId)
                   .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(m => m.MealsCategory)
                   .WithMany(mc => mc.Meals)
                   .HasForeignKey(m => m.MealsCategoryId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasQueryFilter(u => !u.IsDeleted);
        }
    }

}
