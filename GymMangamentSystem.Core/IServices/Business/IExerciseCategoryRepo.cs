using GymMangamentSystem.Core.Dtos.Business;
using GymMangamentSystem.Core.Errors;
using GymMangamentSystem.Core.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangamentSystem.Core.IServices.Business
{
    public interface IExerciseCategoryRepo
    {
        public Task<ApiResponse> AddExerciseCategory(ExerciseCategoryDto exerciseCategoryDto);
        public Task<ApiResponse> DeleteExerciseCategory(int id);
        public Task<ExerciseCategoryDto?> GetExerciseCategory(int id);
        public Task<IEnumerable<ExerciseCategoryDto>> GetExerciseCategories(PaginationParameters? pagination = null);
        public Task<ApiResponse> UpdateExerciseCategory(int id, ExerciseCategoryDto exerciseCategoryDto);

    }
}
