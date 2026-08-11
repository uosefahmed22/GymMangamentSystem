using GymMangamentSystem.Core.IServices;
using GymMangamentSystem.Core.Models.Identity;
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
    public class AppUser : IdentityUser, ISoftDeletable
    {
        public string DisplayName { get; set; } = string.Empty;
        public int UserRole { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string? ProfileImageName { get; set; }
        public string UserCode { get; set; } = string.Empty;
        [NotMapped]
        public IFormFile? Image { get; set; }
        public int? NutritionPlanId { get; set; }
        public NutritionPlan? NutritionPlan { get; set; }
        public int? MembershipId { get; set; }
        public Membership? Membership { get; set; }
        public ICollection<WorkoutPlan> WorkoutPlans { get; set; } = [];
        public ICollection<BMIRecord> BMIRecords { get; set; } = [];
        public ICollection<Feedback> Feedbacks { get; set; } = [];
        public ICollection<Notification> Notifications { get; set; } = [];
        public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    }

}
