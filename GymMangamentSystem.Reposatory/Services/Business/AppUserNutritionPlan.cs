using GymMangamentSystem.Core.Models.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangamentSystem.Reposatory.Services.Business
{
    public class AppUserNutritionPlan
    {

        public string UserId { get; set; }
        public AppUser User { get; set; }

        public int NutritionPlanId { get; set; }
        public NutritionPlan NutritionPlan { get; set; }
    }

}
