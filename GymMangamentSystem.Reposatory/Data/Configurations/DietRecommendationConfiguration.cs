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
    public class DietRecommendationConfiguration : IEntityTypeConfiguration<DietRecommendation>
    {
        public void Configure(EntityTypeBuilder<DietRecommendation> builder)
        {
            builder.HasKey(d => d.DietRecommendationId);

            builder.HasMany(d => d.Users)
                   .WithOne(u => u.DietRecommendation)
                   .HasForeignKey(u => u.DietRecommendationId);
        }
    }
}
