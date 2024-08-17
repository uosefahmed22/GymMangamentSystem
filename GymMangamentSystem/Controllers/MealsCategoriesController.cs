using GymMangamentSystem.Core.Dtos.Business;
using GymMangamentSystem.Core.IServices.Business;
using GymMangamentSystem.Core.Models.Business;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GymMangamentSystem.Apis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MealsCategoriesController : ControllerBase
    {
        private readonly IMealsCategoryRepo _mealsCategoryRepo;

        public MealsCategoriesController(IMealsCategoryRepo mealsCategoryRepo)
        {
            _mealsCategoryRepo = mealsCategoryRepo;
        }
        [HttpGet("GetAllMealsCategories")]
        public async Task<IActionResult> GetAllMealsCategories()
        {
            try
            {
                var mealsCategories =await _mealsCategoryRepo.GetAllMealsCategory();
                return Ok(mealsCategories);
            }
            catch (System.Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpGet("GetMealsCategory")]
        public async Task<IActionResult> GetMealsCategory(int id)
        {
            try
            {
                var mealsCategory = await _mealsCategoryRepo.GetMealsCategoryById(id);
                if (mealsCategory == null)
                {
                    return NotFound($"Meals Category with id {id} not found.");
                }
                return Ok(mealsCategory);
            }
            catch (System.Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [Authorize(Roles = "Admin, Trainer")]
        [HttpPost]
        public async Task<IActionResult> AddMealsCategory([FromForm] MealsCategoryDto mealsCategory)
        {
            try
            {
                await _mealsCategoryRepo.Add(mealsCategory);
                return Ok("Meals Category added successfully.");
            }
            catch (System.Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [Authorize(Roles = "Admin, Trainer")]
        [HttpPut]
        public async Task<IActionResult> UpdateMealsCategory([FromForm] MealsCategoryDto mealsCategory)
        {
            try
            {
                await _mealsCategoryRepo.Update(mealsCategory);
                return Ok("Meals Category updated successfully.");
            }
            catch (System.Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [Authorize(Roles = "Admin, Trainer")]
        [HttpDelete]
        public async Task<IActionResult> DeleteMealsCategory(int id)
        {
            try
            {
                await _mealsCategoryRepo.Delete(id);
                return Ok("Meals Category deleted successfully.");
            }
            catch (System.Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
