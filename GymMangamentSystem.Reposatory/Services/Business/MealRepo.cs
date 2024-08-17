using AutoMapper;
using GymMangamentSystem.Core.Dtos.Business;
using GymMangamentSystem.Core.Errors;
using GymMangamentSystem.Core.IServices;
using GymMangamentSystem.Core.IServices.Business;
using GymMangamentSystem.Core.Models.Business;
using GymMangamentSystem.Reposatory.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangamentSystem.Reposatory.Services.Business
{
    public class MealRepo : IMealRepo
    {
        private readonly AppDBContext _context;
        private readonly IMapper _mapper;
        private readonly IImageService _imageService;

        public MealRepo(AppDBContext context, IMapper mapper, IImageService fileService)
        {
            _context = context;
            _mapper = mapper;
            _imageService = fileService;
        }
        public async Task<ApiResponse> CreateMeal(MealDto meal)
        {

            var existingMeal = _context.Meals.FirstOrDefault(x => x.MealName == meal.MealName);
            if (existingMeal != null)
            {
                return new ApiResponse(400, "Meal already exists");
            }
            try
            {
                if (meal.Image != null)
                {
                    var fileResult = await _imageService.UploadImageAsync(meal.Image);
                    if (fileResult.Item1 == 1)
                    {
                        meal.ImageUrl = fileResult.Item2;
                    }
                    else
                    {
                        return new ApiResponse(400, fileResult.Item2);
                    }
                }
                var mappedMeal = _mapper.Map<Meal>(meal);
                _context.Meals.Add(mappedMeal);
                await _context.SaveChangesAsync();
                return new ApiResponse(200, "Meal added successfully");
            }
            catch (Exception ex)
            {
                return new ApiResponse(500, "Error: " + ex.Message);
            }
        }
        public async Task<ApiResponse> DeleteMeal(int id)
        {
            try
            {
                var meal =await _context.Meals.FirstOrDefaultAsync(x => x.MealId == id);
                if (meal == null || meal.IsDeleted == true)
                {
                    return new ApiResponse(404, "Meal not found");
                }
                 _context.Meals.Remove(meal);
                await _context.SaveChangesAsync();
                return new ApiResponse(200, "Meal deleted successfully");
            }
            catch (Exception ex)
            {
                return new ApiResponse(500, "Error: " + ex.Message);
            }
        }
        public async Task<IEnumerable<MealDto>> GetAllMeals()
        {
            try
            {
                var meals =await _context.Meals.ToListAsync();
                var mappedMeals = _mapper.Map<IEnumerable<MealDto>>(meals);
                return mappedMeals;
            }
            catch (Exception ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
        }
        public async Task<MealDto> GetMealById(int id)
        {
            try
            {
                var meal =await _context.Meals.FirstOrDefaultAsync(x => x.MealId == id);
                var mappedMeal = _mapper.Map<MealDto>(meal);
                return mappedMeal;
            }
            catch (Exception ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
        }
        public async Task<ApiResponse> UpdateMeal(int id, MealDto meal)
        {
            var mealToUpdate = await _context.Meals.FirstOrDefaultAsync(x => x.MealId == id);
            if (mealToUpdate == null || mealToUpdate.IsDeleted == true)
            {
                return new ApiResponse(404, "Meal not found");
            }
            try
            {
                if (meal.Image != null)
                {
                    if (!string.IsNullOrEmpty(mealToUpdate.ImageUrl))
                    {
                        await _imageService.DeleteImageAsync(mealToUpdate.ImageUrl);
                    }
                    var fileResult = await _imageService.UploadImageAsync(meal.Image);
                    if (fileResult.Item1 == 1)
                    {
                        meal.ImageUrl = fileResult.Item2;
                    }
                    else
                    {
                        return new ApiResponse(400, fileResult.Item2);
                    }
                }
                mealToUpdate.MealName = meal.MealName;
                mealToUpdate.Description = meal.Description;
                mealToUpdate.ImageUrl = meal.ImageUrl;
                mealToUpdate.MealsCategoryId = meal.MealsCategoryId;
                mealToUpdate.NutritionPlanId = meal.NutritionPlanId;
                await _context.SaveChangesAsync();
                return new ApiResponse(200, "Meal updated successfully");
            }
            catch (Exception ex)
            {
                return new ApiResponse(500, "Error: " + ex.Message);
            }
        }
    }
}
