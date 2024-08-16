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
    public class ExerciseRepo : IExerciseRepo
    {
        private readonly AppDBContext _context;
        private readonly IMapper _mapper;
        private readonly IImageService _imageService;

        public ExerciseRepo(AppDBContext context, IMapper mapper, IImageService fileService)
        {
            _context = context;
            _mapper = mapper;
            _imageService = fileService;
        }
        public async Task<ApiResponse> AddExercise(ExerciseDto exercise)
        {
            var existingExercise = _context.Exercises.FirstOrDefault(x => x.ExerciseName == exercise.ExerciseName);
            if (existingExercise != null)
            {
                return new ApiResponse(400, "Exercise already exists");
            }
            try
            {
                if (exercise.Image != null)
                {
                    var fileResult = await _imageService.UploadImageAsync(exercise.Image);
                    if (fileResult.Item1 == 1)
                    {
                        exercise.ImageUrl = fileResult.Item2;
                    }
                    else
                    {
                        return new ApiResponse(400, fileResult.Item2);
                    }
                }
                var mappedExercise = _mapper.Map<Exercise>(exercise);
                _context.Exercises.Add(mappedExercise);
                await _context.SaveChangesAsync();
                return new ApiResponse(200, "Exercise added successfully");
            }
            catch (Exception ex)
            {
                return new ApiResponse(500, "Error: " + ex.Message);
            }
        }
        public async Task<ApiResponse> DeleteExercise(int exerciseId)
        {
            var exercise = _context.Exercises.FirstOrDefault(x => x.ExerciseId == exerciseId);
            if (exercise == null)
            {
                return new ApiResponse(400, "Exercise not found");
            }
            try
            {
                _context.Exercises.Remove(exercise);
                await _context.SaveChangesAsync();
                return new ApiResponse(200, "Exercise deleted successfully");
            }
            catch (Exception ex)
            {
                return new ApiResponse(500, "Error: " + ex.Message);
            }
        }
        public async Task<ExerciseDto> GetExerciseById(int exerciseId)
        {
            var exercise =await _context.Exercises.FirstOrDefaultAsync(x => x.ExerciseId == exerciseId);
            if (exercise == null)
            {
                return null;
            }
            try
            {
                var exerciseDto = _mapper.Map<ExerciseDto>(exercise);
                return exerciseDto;
            }
            catch (Exception ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
        }
        public async Task<IEnumerable<ExerciseDto>> GetExerciseList()
        {
            try
            {
                var ExerciseList = await _context.Exercises.Where(x => x.IsDeleted == false).ToListAsync();
                var ExerciseListDto = _mapper.Map<List<ExerciseDto>>(ExerciseList);
                return ExerciseListDto;
            }
            catch (Exception ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
        }
        public async Task<ApiResponse> UpdateExercise(int id ,ExerciseDto exercise)
        {
            var exsistingExercise =await _context.Exercises.FirstOrDefaultAsync(x=>x.ExerciseId==id);
            if(exsistingExercise == null || exsistingExercise.IsDeleted==true)
            {
                return new ApiResponse(400, "Exercise does not exist or already deleted");
            }
            var existingExercise = _context.Exercises.FirstOrDefault(x => x.ExerciseName == exercise.ExerciseName && x.ExerciseId != id);
            if (existingExercise != null)
            {
                return new ApiResponse(400, "Exercise already exists");
            }
            try
            {
                if (exercise.Image != null)
                {
                    if (!string.IsNullOrEmpty(exsistingExercise.ImageUrl))
                    {
                        await _imageService.DeleteImageAsync(exsistingExercise.ImageUrl);
                    }
                    var fileResult = await _imageService.UploadImageAsync(exercise.Image);
                    if (fileResult.Item1 == 1)
                    {
                        exercise.ImageUrl = fileResult.Item2;
                    }
                    else
                    {
                        return new ApiResponse(400, fileResult.Item2);
                    }
                }
                exsistingExercise.ExerciseName = exercise.ExerciseName;
                exsistingExercise.Description = exercise.Description;
                exsistingExercise.ImageUrl = exercise.ImageUrl;
                exsistingExercise.Repetitions = exercise.Repetitions;
                exsistingExercise.Sets = exercise.Sets;
                exsistingExercise.WorkoutPlanId = exercise.WorkoutPlanId;
                exsistingExercise.ExerciseCategoryId = exercise.ExerciseCategoryId;

                await _context.SaveChangesAsync();
                return new ApiResponse(200, "Exercise updated successfully");
            }
            catch (Exception ex)
            {
                return new ApiResponse(500, "Error: " + ex.Message);
            }

        }
    }
}
