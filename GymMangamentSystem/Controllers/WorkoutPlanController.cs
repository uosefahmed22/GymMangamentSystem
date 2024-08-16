using GymMangamentSystem.Core.Dtos.Business;
using GymMangamentSystem.Core.Errors;
using GymMangamentSystem.Core.IServices.Business;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GymMangamentSystem.Apis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkoutPlanController : ControllerBase
    {
        private readonly IWorkoutPlanRepo _workoutPlanRepo;

        public WorkoutPlanController(IWorkoutPlanRepo workoutPlanRepo)
        {
            _workoutPlanRepo = workoutPlanRepo;
        }
        [HttpGet("getWorkoutPlans")]
        public async Task<IActionResult> GetWorkoutPlans()
        {
            var response = await _workoutPlanRepo.GetWorkoutPlans();
            try
            {
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse(404, ex.Message));
            }
        }
        [HttpGet("getWorkoutPlan")]
        public async Task<IActionResult> GetWorkoutPlan(int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = await _workoutPlanRepo.GetWorkoutPlan(id);
                if (result == null)
                {
                    return NotFound($"Workout Plan with id {id} not found.");
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse(404, ex.Message));
            }
        }
        [Authorize(Roles = "Admin, Trainer")]
        [HttpPost]
        public async Task<IActionResult> CreateWorkoutPlan([FromForm] WorkoutPlanDto workoutPlanDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var trainerIdClaim = User.Claims.FirstOrDefault(c => c.Type == "TrainerId");
            if (trainerIdClaim == null)
            {
                return BadRequest("TrainerId claim not found in the token");
            }
            workoutPlanDto.TrainerId = trainerIdClaim.Value;
            var response = await _workoutPlanRepo.CreateWorkoutPlan(workoutPlanDto);
            if (response.StatusCode == 200)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
        [Authorize(Roles = "Admin, Trainer")]
        [HttpDelete]
        public async Task<IActionResult> DeleteWorkoutPlan(int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = await _workoutPlanRepo.DeleteWorkoutPlan(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse(404, ex.Message));
            }
        }
        [Authorize(Roles="Admin, Trainer")]
        [HttpPut]
        public async Task<IActionResult> UpdateWorkoutPlan(int id, [FromForm] WorkoutPlanDto workoutPlanDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = await _workoutPlanRepo.UpdateWorkoutPlan(id, workoutPlanDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse(404, ex.Message));
            }
        }
    }
}