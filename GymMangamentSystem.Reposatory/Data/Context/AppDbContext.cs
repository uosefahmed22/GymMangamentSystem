using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using GymMangamentSystem.Core.Models.Business;
using GymMangamentSystem.Core.Dtos.Business;

namespace GymMangamentSystem.Reposatory.Data.Context
{
    public class AppDBContext : IdentityDbContext<AppUser>
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            SeedRoles(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
        private static void SeedRoles(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IdentityRole>().HasData
            (
                new IdentityRole { Id = "1", Name = "Admin", NormalizedName = "ADMIN" },
                new IdentityRole { Id = "2", Name = "Trainer", NormalizedName = "TRAINER" },
                new IdentityRole { Id = "3", Name = "Member", NormalizedName = "MEMBER" },
                new IdentityRole { Id = "5", Name = "Receptionist", NormalizedName = "RECEPTIONIST" }
            );
        }


        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<Exercise> Exercises { get; set; }
        public DbSet<ExerciseCategory> ExerciseCategories { get; set; }
        public DbSet<MealsCategory> MealsCategories { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<BMIRecord> bMIRecords { get; set; }
        public DbSet<Meal> Meals { get; set; }
        public DbSet<Membership> Memberships { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<NutritionPlan> NutritionPlans { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<WorkoutPlan> WorkoutPlans { get; set; }
    }
}
