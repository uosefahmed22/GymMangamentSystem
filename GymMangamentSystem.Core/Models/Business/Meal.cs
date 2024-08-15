using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangamentSystem.Core.Models.Business
{
    public class Meal
    {
        public int MealId { get; set; }
        public string? ImageUrl { get; set; }
        [NotMapped]
        public IFormFile Image { get; set; }
        public string MealName { get; set; }
        public string Description { get; set; }
        public bool IsDeleted { get; set; }
        public int? NutritionPlanId { get; set; }
        public NutritionPlan? NutritionPlan { get; set; }
        public int MealsCategoryId { get; set; }
        public MealsCategory MealsCategory { get; set; }
    }
}
