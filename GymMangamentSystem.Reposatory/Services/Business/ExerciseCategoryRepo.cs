﻿using AutoMapper;
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
    public class ExerciseCategoryRepo : IExerciseCategoryRepo
    {
        private readonly AppDBContext _context;
        private readonly IMapper _mapper;
        private readonly IImageService _imageService;

        public ExerciseCategoryRepo(AppDBContext context, IMapper mapper, IImageService fileService)
        {
            _context = context;
            _mapper = mapper;
            _imageService = fileService;
        }
        public async Task<ApiResponse> AddExerciseCategory(ExerciseCategoryDto exerciseCategoryDto)
        {
            var existingCategory =await _context.ExerciseCategories.FirstOrDefaultAsync(x => x.CategoryName == exerciseCategoryDto.CategoryName);
            if (exerciseCategoryDto == null || existingCategory != null)
            {
                return new ApiResponse(400, "Exercise Category is null or already exists");
            }

            try
            {
                if (exerciseCategoryDto.Image != null)
                {
                    var fileResult = await _imageService.UploadImageAsync(exerciseCategoryDto.Image);
                    if (fileResult.Item1 == 1)
                    {
                        exerciseCategoryDto.ImageUrl = fileResult.Item2;
                    }
                    else
                    {
                        return new ApiResponse(400, fileResult.Item2);
                    }
                }

                var exerciseCategory = _mapper.Map<ExerciseCategory>(exerciseCategoryDto);
                _context.ExerciseCategories.Add(exerciseCategory);
                await _context.SaveChangesAsync();

                return new ApiResponse(200, "Exercise Category added successfully");
            }
            catch (Exception ex)
            {
                return new ApiResponse(500, "Error: " + ex.Message);
            }
        }
        public async Task<ApiResponse> DeleteExerciseCategory(int id)
        {
            var exerciseCategory =await _context.ExerciseCategories.FindAsync(id);
            if (exerciseCategory == null || exerciseCategory.IsDeleted == true)
            {
                return new ApiResponse(404, "Exercise Category not found");
            }
            try
            {
                exerciseCategory.IsDeleted = true;
                _context.Update(exerciseCategory);
                await _context.SaveChangesAsync();
                return new ApiResponse(200, "Exercise Category deleted successfully");
            }
            catch (Exception ex)
            {
                return new ApiResponse(400, "Error: " + ex.Message);
            }

        }
        public async Task<IEnumerable<ExerciseCategoryDto>> GetExerciseCategories()
        {
            try
            {
                var exerciseCategories = await _context.ExerciseCategories.Where(x => x.IsDeleted == false).ToListAsync();
                var exerciseCategoriesDto = _mapper.Map<List<ExerciseCategoryDto>>(exerciseCategories);
                return exerciseCategoriesDto;
            }
            catch (Exception ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
        }
        public async Task<ExerciseCategoryDto> GetExerciseCategory(int id)
        {
            try
            {
                var exerciseCategory = await _context.ExerciseCategories.FindAsync(id);
                if (exerciseCategory == null || exerciseCategory.IsDeleted == true)
                {
                    return null;
                }
                var exerciseCategoryDto = _mapper.Map<ExerciseCategoryDto>(exerciseCategory);
                return exerciseCategoryDto;
            }
            catch (Exception ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
        }
        public async Task<ApiResponse> UpdateExerciseCategory(int id, ExerciseCategoryDto exerciseCategoryDto)
        {
            var existingExerciseCategory = await _context.ExerciseCategories.FindAsync(id);
            if (existingExerciseCategory == null || existingExerciseCategory.IsDeleted)
            {
                return new ApiResponse(404, "Exercise Category not found");
            }

            if (exerciseCategoryDto.Image != null)
            {
                if (!string.IsNullOrEmpty(existingExerciseCategory.ImageUrl))
                {
                    await _imageService.DeleteImageAsync(existingExerciseCategory.ImageUrl);
                }

                var fileResult = await _imageService.UploadImageAsync(exerciseCategoryDto.Image);
                if (fileResult.Item1 == 1)
                {
                    existingExerciseCategory.ImageUrl = fileResult.Item2;
                }
                else
                {
                    return new ApiResponse(400, fileResult.Item2);
                }
            }
            else
            {
                exerciseCategoryDto.ImageUrl = existingExerciseCategory.ImageUrl;
            }

            existingExerciseCategory.CategoryName = exerciseCategoryDto.CategoryName ?? existingExerciseCategory.CategoryName;
            existingExerciseCategory.ImageUrl = exerciseCategoryDto.ImageUrl ?? existingExerciseCategory.ImageUrl;

            await _context.SaveChangesAsync();

            return new ApiResponse(200, "Exercise Category updated successfully");
        }
    }
}