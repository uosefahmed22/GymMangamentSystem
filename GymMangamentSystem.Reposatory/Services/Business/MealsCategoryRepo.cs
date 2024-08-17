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
    public class MealsCategoryRepo : IMealsCategoryRepo
    {
        private readonly AppDBContext _context;
        private readonly IMapper _mapper;
        private readonly IImageService _imageService;

        public MealsCategoryRepo(AppDBContext context, IMapper mapper, IImageService fileService)
        {
            _context = context;
            _mapper = mapper;
            _imageService = fileService;
        }

        public async Task<ApiResponse> Add(MealsCategoryDto mealsCategory)
        {
            try
            {
                var existingCategory = _context.MealsCategories.FirstOrDefault(x => x.CategoryName == mealsCategory.CategoryName);
                if (existingCategory != null)
                {
                    return new ApiResponse(400, "Meals Category already exists");
                }

                if (mealsCategory.Image != null)
                {
                    var fileResult = await _imageService.UploadImageAsync(mealsCategory.Image);
                    if (fileResult.Item1 == 1)
                    {
                        mealsCategory.ImageUrl = fileResult.Item2;
                    }
                    else
                    {
                        return new ApiResponse(400, fileResult.Item2);
                    }
                }

                var mappedCategory = _mapper.Map<MealsCategory>(mealsCategory);
                _context.MealsCategories.Add(mappedCategory);
                await _context.SaveChangesAsync();

                return new ApiResponse(200, "Meals Category added successfully");
            }
            catch (Exception ex)
            {
                return new ApiResponse(500, "Error: " + ex.Message);
            }
        }
        public async Task<ApiResponse> Delete(int mealsCategoryId)
        {
            var ExsisitingCategory = await _context.MealsCategories.FirstOrDefaultAsync(x => x.MealsCategoryId == mealsCategoryId);
            if (ExsisitingCategory == null || ExsisitingCategory.IsDeleted == true)
            {
                return new ApiResponse(400, "Meals Category not found");
            }
            try
            {
                ExsisitingCategory.IsDeleted = true;
                await _context.SaveChangesAsync();
                return new ApiResponse(200, "Meals Category deleted successfully");
            }
            catch (Exception ex)
            {
                return new ApiResponse(500, "Error: " + ex.Message);
            }
        }
        public async Task<IEnumerable<MealsCategoryDto>> GetAllMealsCategory()
        {
            try
            {
                var mealsCategories = await _context.MealsCategories.Where(x => x.IsDeleted == false).ToListAsync();
                var mappedCategories = _mapper.Map<List<MealsCategoryDto>>(mealsCategories);
                return mappedCategories;
            }
            catch (Exception ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
        }
        public async Task<MealsCategoryDto> GetMealsCategoryById(int mealsCategoryId)
        {
            try
            {
                var mealsCategory = await _context.MealsCategories.Where(x => x.IsDeleted == false).FirstOrDefaultAsync(x => x.MealsCategoryId == mealsCategoryId);
                var mappedCategory = _mapper.Map<MealsCategoryDto>(mealsCategory);
                return mappedCategory;
            }
            catch (Exception ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
        }
        public async Task<ApiResponse> Update(MealsCategoryDto mealsCategory)
        {
            try
            {
                var existingCategory = await _context.MealsCategories.FirstOrDefaultAsync(x => x.MealsCategoryId == mealsCategory.MealsCategoryId);
                if (existingCategory == null || existingCategory.IsDeleted == true)
                {
                    return new ApiResponse(400, "Meals Category not found");
                }
                if (existingCategory.Image != null)
                {
                    if (!string.IsNullOrEmpty(existingCategory.ImageUrl))
                    {
                        await _imageService.DeleteImageAsync(existingCategory.ImageUrl);
                    }

                    var fileResult = await _imageService.UploadImageAsync(existingCategory.Image);
                    if (fileResult.Item1 == 1)
                    {
                        existingCategory.ImageUrl = fileResult.Item2;
                    }
                    else
                    {
                        return new ApiResponse(400, fileResult.Item2);
                    }
                }
                else
                {
                    existingCategory.ImageUrl = existingCategory.ImageUrl;
                }
                existingCategory.CategoryName = mealsCategory.CategoryName;
                await _context.SaveChangesAsync();
                return new ApiResponse(200, "Meals Category updated successfully");
            }
            catch (Exception ex)
            {
                return new ApiResponse(500, "Error: " + ex.Message);
            }
        }
    }
}
