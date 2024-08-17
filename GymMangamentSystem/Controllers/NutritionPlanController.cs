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
    public class NutritionPlanController : ControllerBase
    {
        private readonly INutritionPlanRepo _nutritionPlanRepo;

        public NutritionPlanController(INutritionPlanRepo nutritionPlanRepo)
        {
            _nutritionPlanRepo = nutritionPlanRepo;
        }
        [HttpGet("GetNutritionPlans")]
        public async Task<IActionResult> GetNutritionPlans()
        {
            try
            {
                var nutritionPlans = await _nutritionPlanRepo.GetNutritionPlans();
                return Ok(nutritionPlans);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("GetNutritionPlan")]
        public async Task<IActionResult> GetNutritionPlan(int nutritionPlanId)
        {
            try
            {
                var nutritionPlan = await _nutritionPlanRepo.GetNutritionPlan(nutritionPlanId);
                return Ok(nutritionPlan);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Authorize(Roles = "Admin,Trainer")]
        [HttpPost]
        public async Task<IActionResult> CreateNutritionPlan([FromForm] NutritionPlanDto nutritionPlanDto)
        {
            try
            {
                var response = await _nutritionPlanRepo.CreateNutritionPlan(nutritionPlanDto);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Authorize(Roles = "Admin,Trainer")]
        [HttpPut]
        public async Task<IActionResult> UpdateNutritionPlan(int nutritionPlanId, [FromForm] NutritionPlanDto nutritionPlanDto)
        {
            try
            {
                var response = await _nutritionPlanRepo.UpdateNutritionPlan(nutritionPlanId, nutritionPlanDto);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Authorize(Roles = "Admin,Trainer")]
        [HttpDelete]
        public async Task<IActionResult> DeleteNutritionPlan(int nutritionPlanId)
        {
            try
            {
                var response = await _nutritionPlanRepo.DeleteNutritionPlan(nutritionPlanId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
