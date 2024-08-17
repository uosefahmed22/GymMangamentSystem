using GymMangamentSystem.Core.Dtos.Business;
using GymMangamentSystem.Core.Errors;
using GymMangamentSystem.Core.Models.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangamentSystem.Core.IServices.Business
{
    public interface INutritionPlanRepo
    {
        Task<ApiResponse> CreateNutritionPlan(NutritionPlanDto nutritionPlanDto);
        Task<ApiResponse> UpdateNutritionPlan(int nutritionPlanId, NutritionPlanDto nutritionPlanDto);
        Task<ApiResponse> DeleteNutritionPlan(int nutritionPlanId);
        Task<NutritionPlanDto> GetNutritionPlan(int nutritionPlanId);
        Task<IEnumerable<NutritionPlanDto>> GetNutritionPlans();
    }
}
