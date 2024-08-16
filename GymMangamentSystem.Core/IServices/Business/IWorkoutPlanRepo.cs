using GymMangamentSystem.Core.Dtos.Business;
using GymMangamentSystem.Core.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangamentSystem.Core.IServices.Business
{
    public interface IWorkoutPlanRepo
    {
        Task<ApiResponse> CreateWorkoutPlan(WorkoutPlanDto workoutPlanDto);
        Task<ApiResponse> UpdateWorkoutPlan(int id, WorkoutPlanDto workoutPlanDto);
        Task<ApiResponse> DeleteWorkoutPlan(int workoutPlanId);
        Task<WorkoutPlanDto> GetWorkoutPlan(int workoutPlanId);
        Task<IEnumerable<WorkoutPlanDto>> GetWorkoutPlans();
    }
}
