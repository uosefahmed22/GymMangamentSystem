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

    public class MealsCategoryConfiguration : IEntityTypeConfiguration<MealsCategory>
    {
        public void Configure(EntityTypeBuilder<MealsCategory> builder)
        {
            builder.HasKey(mc => mc.MealsCategoryId);
            builder.HasMany(mc => mc.Meals)
                   .WithOne(m => m.MealsCategory)
                   .HasForeignKey(m => m.MealsCategoryId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasQueryFilter(u => !u.IsDeleted);
        }
    }

}
