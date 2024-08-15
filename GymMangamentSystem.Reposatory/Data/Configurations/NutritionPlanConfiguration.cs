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
    public class NutritionPlanConfiguration : IEntityTypeConfiguration<NutritionPlan>
    {
        public void Configure(EntityTypeBuilder<NutritionPlan> builder)
        {
            builder.HasKey(np => np.NutritionPlanId);
            builder.HasMany(np => np.Users)
                   .WithMany(u => u.NutritionPlans);

            builder.HasMany(np => np.Meals)
                   .WithOne(m => m.NutritionPlan)
                   .HasForeignKey(m => m.NutritionPlanId)
                   .OnDelete(DeleteBehavior.SetNull);

            builder.HasQueryFilter(u => !u.IsDeleted);
        }
    }


}
