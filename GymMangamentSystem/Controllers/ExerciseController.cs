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
    public class ExerciseController : ControllerBase
    {
        private readonly IExerciseRepo _exerciseRepo;

        public ExerciseController(IExerciseRepo exerciseRepo)
        {
            _exerciseRepo = exerciseRepo;
        }
        [HttpGet("getExerciseList")]
        public async Task<IActionResult> GetExerciseList()
        {
            var result = await _exerciseRepo.GetExerciseList();
            try
            {
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse(404, ex.Message));
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetExerciseById(int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = await _exerciseRepo.GetExerciseById(id);
                if (result == null)
                {
                    return NotFound($"Exercise with id {id} not found.");
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse(404, ex.Message));
            }
        }
        [Authorize(Roles="Admin, Trainer")]
        [HttpPost]
        public async Task<IActionResult> AddExercise([FromForm] ExerciseDto exercise)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _exerciseRepo.AddExercise(exercise);
            if (result.StatusCode == 200)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [Authorize(Roles = "Admin, Trainer")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateExercise(int id, [FromForm] ExerciseDto exercise)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _exerciseRepo.UpdateExercise(id, exercise);
            if (result.StatusCode == 200)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [Authorize(Roles = "Admin, Trainer")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExercise(int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = await _exerciseRepo.DeleteExercise(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse(404, ex.Message));
            }
        }
    }
}
