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
    public class BMIRecordConfiguration : IEntityTypeConfiguration<BMIRecord>
    {
        public void Configure(EntityTypeBuilder<BMIRecord> builder)
        {
            builder.ToTable("BMIRecords");

            builder.HasKey(b => b.BMIRecordId);

            builder.Property(b => b.BMIRecordId)
                .IsRequired();

            builder.Property(b => b.Category)
                .IsRequired()
                .HasConversion<string>(); // Storing enum as string

            builder.Property(b => b.MeasurementDate)
                .IsRequired()
                .HasDefaultValueSql("GETDATE()");

            builder.Property(b => b.IsDeleted)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(b => b.WeightInKg)
                .IsRequired()
                .HasPrecision(5, 2);

            builder.Property(b => b.HeightInMeters)
                .IsRequired()
               //by cm not meter
                .HasPrecision(5, 2);//from 0 to 999.99

            builder.HasOne(b => b.User)
                .WithMany(u => u.BMIRecords)
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasQueryFilter(b => !b.IsDeleted);

            builder.HasIndex(b => b.UserId);
        }
    }

}
