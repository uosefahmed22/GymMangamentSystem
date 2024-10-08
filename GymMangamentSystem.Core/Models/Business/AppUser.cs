﻿using GymMangamentSystem.Core.Models.Identity;
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
        public string UserCode { get; set; }
        [NotMapped]
        public IFormFile? Image { get; set; }
        public int? NutritionPlanId { get; set; }
        public NutritionPlan nutritionPlan { get; set; }
        public int? MembershipId { get; set; }
        public Membership membership { get; set; }
        public ICollection<WorkoutPlan> WorkoutPlans { get; set; }
        public ICollection<BMIRecord> BMIRecords { get; set; }
        public ICollection<Feedback> Feedbacks { get; set; }
        public ICollection<Notification> Notifications { get; set; }
        public ICollection<RefreshToken> RefreshTokens { get; set; }
    }

}
