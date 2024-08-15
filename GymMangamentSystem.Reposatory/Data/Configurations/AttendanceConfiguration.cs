using GymMangamentSystem.Core.Models.Business;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Org.BouncyCastle.Crypto;
using System.ComponentModel;
using System.Reflection;
using System.Security.AccessControl;

namespace GymMangamentSystem.Reposatory.Data.Configurations
{
    public class AttendanceConfiguration : IEntityTypeConfiguration<Attendance>
    {
        public void Configure(EntityTypeBuilder<Attendance> builder)
        {
            builder.HasKey(a => a.AttendanceId);
            builder.HasOne(a => a.User)
                   .WithMany(u => u.Attendances)
                   .HasForeignKey(a => a.UserId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(a => a.Class)
                   .WithMany(c => c.Attendances)
                   .HasForeignKey(a => a.ClassId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasQueryFilter(a => !a.IsDeleted);
        }
    }

}
