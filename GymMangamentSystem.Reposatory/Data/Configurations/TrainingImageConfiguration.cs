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
    public class TrainingImageConfiguration : IEntityTypeConfiguration<TrainingImage>
    {
        public void Configure(EntityTypeBuilder<TrainingImage> builder)
        {
            builder.HasKey(t => t.TrainingImageId);
            builder.HasOne(t => t.Exercise)
                   .WithMany(e => e.TrainingImages)
                   .HasForeignKey(t => t.ExerciseId);
        }
    }
}
