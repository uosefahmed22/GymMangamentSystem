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
    public class EnrollmentConfiguration : IEntityTypeConfiguration<Enrollment>
    {
        public void Configure(EntityTypeBuilder<Enrollment> builder)
        {
            builder.HasKey(e => e.EnrollmentId);
            builder.HasOne(e => e.User)
                   .WithMany(u => u.Enrollments)
                   .HasForeignKey(e => e.UserId);
            builder.HasOne(e => e.Class)
                   .WithMany(c => c.Enrollments)
                   .HasForeignKey(e => e.ClassId);
        }
    }
}
