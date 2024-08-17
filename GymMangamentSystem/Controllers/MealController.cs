using GymMangamentSystem.Core.Dtos.Business;
using GymMangamentSystem.Core.IServices.Business;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GymMangamentSystem.Apis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MealController : ControllerBase
    {
        private readonly IMealRepo _mealRepo;

        public MealController(IMealRepo mealRepo)
        {
            _mealRepo = mealRepo;
        }
        [HttpGet("GetAllMeals")]
        public async Task<IActionResult> GetAllMeals()
        {
            try
            {
                var meals = await _mealRepo.GetAllMeals();
                return Ok(meals);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("GetMealById")]
        public async Task<IActionResult> GetMealById(int id)
        {
            try
            {
                var meal = await _mealRepo.GetMealById(id);
                return Ok(meal);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Authorize(Roles = "Admin, Trainer")]
        [HttpPost]
        public async Task<IActionResult> CreateMeal([FromForm] MealDto meal)
        {
            try
            {
                var response = await _mealRepo.CreateMeal(meal);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Authorize(Roles = "Admin, Trainer")]
        [HttpPut]
        public async Task<IActionResult> UpdateMeal(int id, [FromForm] MealDto meal)
        {
            try
            {
                var response = await _mealRepo.UpdateMeal(id, meal);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Authorize(Roles = "Admin, Trainer")]
        [HttpDelete]
        public async Task<IActionResult> DeleteMeal(int id)
        {
            try
            {
                var response = await _mealRepo.DeleteMeal(id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
