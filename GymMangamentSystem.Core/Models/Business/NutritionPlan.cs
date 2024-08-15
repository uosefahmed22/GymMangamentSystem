using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangamentSystem.Core.Models.Business
{
    public class NutritionPlan
    {
        public int NutritionPlanId { get; set; }
        public string? ImageUrl { get; set; }
        [NotMapped]
        public IFormFile Image { get; set; }
        public string PlanName { get; set; }
        public string Description { get; set; }
        public bool IsDeleted { get; set; }
        public ICollection<AppUser> Users { get; set; }
        public ICollection<Meal> Meals { get; set; }
    }
}
