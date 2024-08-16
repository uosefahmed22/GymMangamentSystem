using GymMangamentSystem.Core.Dtos.Business;
using GymMangamentSystem.Core.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangamentSystem.Core.IServices.Business
{
    public interface IExerciseRepo
    {
        Task<ApiResponse> AddExercise(ExerciseDto exercise);
        Task<ApiResponse> UpdateExercise(int id, ExerciseDto exercise);
        Task<ApiResponse> DeleteExercise(int exerciseId);
        Task<IEnumerable<ExerciseDto>> GetExerciseList();
        Task<ExerciseDto> GetExerciseById(int exerciseId);
    }
}
