using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangamentSystem.Core.Models.Business
{
    public class AppUser : IdentityUser
    {
        public string DisplayName { get; set; }
        public int UserRole { get; set; }
        public string? ProfileImageName { get; set; }

        [NotMapped]
        public IFormFile? Image { get; set; }
        public int? DietRecommendationId { get; set; }
        public DietRecommendation DietRecommendation { get; set; }
        public ICollection<Enrollment> Enrollments { get; set; }
        public ICollection<Attendance> Attendances { get; set; }
        public ICollection<HealthMetric> HealthMetrics { get; set; }
        public ICollection<Feedback> Feedbacks { get; set; }
        public ICollection<Notification> Notifications { get; set; }
        public ICollection<Membership> Memberships { get; set; }
        public int? NutritionPlanId { get; set; }
        public NutritionPlan NutritionPlan { get; set; }
        public ICollection<AppUserWorkoutPlan> WorkoutPlans { get; set; }
    }

}
