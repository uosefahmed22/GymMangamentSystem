using GymMangamentSystem.Core.Dtos.Business;
using GymMangamentSystem.Core.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangamentSystem.Core.IServices.Business
{
    public interface IMealRepo
    {
        Task<IEnumerable<MealDto>> GetAllMeals();
        Task<MealDto> GetMealById(int id);
        Task<ApiResponse> CreateMeal(MealDto meal);
        Task<ApiResponse> UpdateMeal(int id, MealDto meal);
        Task<ApiResponse> DeleteMeal(int id);
    }
}
