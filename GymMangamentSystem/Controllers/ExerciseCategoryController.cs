using AutoMapper;
using GymMangamentSystem.Core.Dtos.Business;
using GymMangamentSystem.Core.Errors;
using GymMangamentSystem.Core.IServices;
using GymMangamentSystem.Core.IServices.Business;
using GymMangamentSystem.Core.Models.Business;
using GymMangamentSystem.Reposatory.Data.Context;
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
        private readonly IMapper _mapper;
        private readonly AppDBContext _context;

        public ExerciseCategoryController(IExerciseCategoryRepo exerciseCategory, IImageService fileService, IMapper mapper, AppDBContext context)
        {
            _exerciseCategory = exerciseCategory;
            _imageService = fileService;
            _mapper = mapper;
            _context = context;
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
        [HttpPost]
        public async Task<IActionResult> AddExerciseCategory([FromForm] ExerciseCategoryDto exerciseCategory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (exerciseCategory.Image != null)
            {
                var fileResult = await _imageService.UploadImageAsync(exerciseCategory.Image);
                if (fileResult.Item1 == 1)
                {
                    exerciseCategory.ImageUrl = fileResult.Item2;
                }
                else
                {
                    return BadRequest(new ApiResponse(400, fileResult.Item2));
                }
            }

            var response = await _exerciseCategory.AddExerciseCategory(exerciseCategory);
            if (response.StatusCode == 200)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }
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
        [HttpPut]
        public async Task<IActionResult> UpdateExerciseCategory(int id, [FromForm] ExerciseCategoryDto exerciseCategory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse(400, "Invalid data provided"));
            }

            var existingExerciseCategory = await _context.ExerciseCategories.FindAsync(id);
            if (existingExerciseCategory == null || existingExerciseCategory.IsDeleted)
            {
                return NotFound(new ApiResponse(404, "Exercise Category not found"));
            }

            if (exerciseCategory.Image != null)
            {
                // Delete old image if it exists
                if (!string.IsNullOrEmpty(existingExerciseCategory.ImageUrl))
                {
                    await _imageService.DeleteImageAsync(existingExerciseCategory.ImageUrl);
                }

                // Upload new image
                var fileResult = await _imageService.UploadImageAsync(exerciseCategory.Image);
                if (fileResult.Item1 == 1)
                {
                    existingExerciseCategory.ImageUrl = fileResult.Item2;
                }
                else
                {
                    return BadRequest(new ApiResponse(400, fileResult.Item2));
                }
            }

            // Update other properties
            existingExerciseCategory.CategoryName = exerciseCategory.CategoryName ?? existingExerciseCategory.CategoryName;

            _context.ExerciseCategories.Update(existingExerciseCategory);
            await _context.SaveChangesAsync();

            return Ok(new ApiResponse(200, "Exercise Category updated successfully."));
        }




    }
}