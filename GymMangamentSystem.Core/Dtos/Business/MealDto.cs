using GymMangamentSystem.Core.Models.Business;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangamentSystem.Core.Dtos.Business
{
    public class MealDto
    {
        public int MealId { get; set; }
        public string? ImageUrl { get; set; }
        [NotMapped]
        public IFormFile? Image { get; set; }
        public string MealName { get; set; }
        public string Description { get; set; }
        public int? NutritionPlanId { get; set; }
        public int MealsCategoryId { get; set; }
    }
}
