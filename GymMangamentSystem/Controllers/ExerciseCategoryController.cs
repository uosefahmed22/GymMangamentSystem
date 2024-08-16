using AutoMapper;
using Azure;
using GymMangamentSystem.Core.Dtos.Business;
using GymMangamentSystem.Core.Errors;
using GymMangamentSystem.Core.IServices;
using GymMangamentSystem.Core.IServices.Business;
using GymMangamentSystem.Core.Models.Business;
using GymMangamentSystem.Reposatory.Data.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GymMangamentSystem.Apis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExerciseCategoryController : ControllerBase
    {
        private readonly IExerciseCategoryRepo _exerciseCategory;
        private readonly IImageService _imageService;

        public ExerciseCategoryController(IExerciseCategoryRepo exerciseCategory, IImageService fileService)
        {
            _exerciseCategory = exerciseCategory;
            _imageService = fileService;
        }
        [HttpGet("getExerciseCategories")]
        public async Task<IActionResult> GetExerciseCategories()
        {
            var response = await _exerciseCategory.GetExerciseCategories();
            try
            {
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse(404, ex.Message));
            }
        }
        [HttpGet("getExerciseCategory")]
        public async Task<IActionResult> GetExerciseCategory(int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = await _exerciseCategory.GetExerciseCategory(id);
                if (result == null)
                {
                    return NotFound($"Exercise Category with id {id} not found.");
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
        public async Task<IActionResult> AddExerciseCategory([FromForm] ExerciseCategoryDto exerciseCategory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _exerciseCategory.AddExerciseCategory(exerciseCategory);

            if (response.StatusCode == 200)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }
        [Authorize(Roles = "Admin, Trainer")]
        [HttpDelete]
        public async Task<IActionResult> DeleteExerciseCategory(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var response = await _exerciseCategory.DeleteExerciseCategory(id);
            if (response.StatusCode == 200)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
        [Authorize(Roles = "Admin, Trainer")]
        [HttpPut]
        public async Task<IActionResult> UpdateExerciseCategory(int id, [FromForm] ExerciseCategoryDto exerciseCategory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse(400, "Invalid data provided"));
            }
            var result = await _exerciseCategory.UpdateExerciseCategory(id, exerciseCategory);
            if (result.StatusCode == 200)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}