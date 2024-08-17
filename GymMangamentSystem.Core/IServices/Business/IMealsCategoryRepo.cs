using GymMangamentSystem.Core.Dtos.Business;
using GymMangamentSystem.Core.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangamentSystem.Core.IServices.Business
{
    public interface IMealsCategoryRepo
    {
        Task<ApiResponse> Add(MealsCategoryDto mealsCategory);
        Task<ApiResponse> Update(MealsCategoryDto mealsCategory);
        Task<ApiResponse> Delete(int mealsCategoryId);
        Task<IEnumerable<MealsCategoryDto>> GetAllMealsCategory();
        Task<MealsCategoryDto> GetMealsCategoryById(int mealsCategoryId);
    }
}
