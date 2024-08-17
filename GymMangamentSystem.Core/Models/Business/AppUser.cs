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
        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string? ProfileImageName { get; set; }
        [NotMapped]
        public IFormFile? Image { get; set; }
        public ICollection<WorkoutPlan> WorkoutPlans { get; set; }
        public ICollection<NutritionPlan> NutritionPlans { get; set; }
        public ICollection<Attendance> Attendances { get; set; }
        public ICollection<BMIRecord> BMIRecords { get; set; }
        public ICollection<Feedback> Feedbacks { get; set; }
        public ICollection<Notification> Notifications { get; set; }
        public ICollection<Membership> Memberships { get; set; }
    }

}
