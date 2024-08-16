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
    public class WorkoutPlanRepo : IWorkoutPlanRepo
    {
        private readonly AppDBContext _context;
        private readonly IMapper _mapper;
        private readonly IImageService _imageService;

        public WorkoutPlanRepo(AppDBContext context, IMapper mapper, IImageService fileService)
        {
            _context = context;
            _mapper = mapper;
            _imageService = fileService;
        }
        public async Task<ApiResponse> CreateWorkoutPlan(WorkoutPlanDto workoutPlanDto)
        {
            var exsistingWorkoutPlan = _context.WorkoutPlans.FirstOrDefault(x => x.PlanName == workoutPlanDto.PlanName);
            if (workoutPlanDto == null || exsistingWorkoutPlan != null)
            {
                return new ApiResponse(400, "Workout Plan is null or already exists");
            }
            try
            {
                if (workoutPlanDto.Image != null)
                {
                    var fileResult = await _imageService.UploadImageAsync(workoutPlanDto.Image);
                    if (fileResult.Item1 == 1)
                    {
                        workoutPlanDto.ImageUrl = fileResult.Item2;
                    }
                    else
                    {
                        return new ApiResponse(400, fileResult.Item2);
                    }
                }
                var workoutPlan = _mapper.Map<WorkoutPlan>(workoutPlanDto);
                _context.WorkoutPlans.Add(workoutPlan);
                await _context.SaveChangesAsync();
                return new ApiResponse(200, "Workout Plan added successfully");
            }
            catch (Exception ex)
            {
                return new ApiResponse(500, "Error: " + ex.Message);
            }
        }
        public async Task<ApiResponse> DeleteWorkoutPlan(int workoutPlanId)
        {
            var exsistingWorkoutPlan = _context.WorkoutPlans.FirstOrDefault(x => x.WorkoutPlanId == workoutPlanId);
            if (exsistingWorkoutPlan == null || exsistingWorkoutPlan.IsDeleted == true)
            {
                return new ApiResponse(400, "Workout Plan does not exist or already deleted");
            }
            try
            {
                exsistingWorkoutPlan.IsDeleted = true;
                await _context.SaveChangesAsync();
                return new ApiResponse(200, "Workout Plan deleted successfully");
            }
            catch (Exception ex)
            {
                return new ApiResponse(500, "Error: " + ex.Message);
            }
        }
        public async Task<WorkoutPlanDto> GetWorkoutPlan(int workoutPlanId)
        {
            var workoutPlan =await _context.WorkoutPlans.FirstOrDefaultAsync(x => x.WorkoutPlanId == workoutPlanId);
            if (workoutPlan == null)
            {
                return null;
            }
            try
            {
                var workoutPlanDto = _mapper.Map<WorkoutPlanDto>(workoutPlan);
                return workoutPlanDto;
            }
            catch (Exception ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
        }
        public async Task<IEnumerable<WorkoutPlanDto>> GetWorkoutPlans()
        {
            try
            {
                var workoutPlans =await _context.WorkoutPlans.Where(x => x.IsDeleted == false).ToListAsync();
                var workoutPlansDto = _mapper.Map<List<WorkoutPlanDto>>(workoutPlans);
                return workoutPlansDto;
            }
            catch (Exception ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
        }
        public async Task<ApiResponse> UpdateWorkoutPlan(int id, WorkoutPlanDto workoutPlanDto)
        {
            var existingWorkoutPlan = await _context.WorkoutPlans.FirstOrDefaultAsync(x => x.WorkoutPlanId == id);
            if (existingWorkoutPlan == null || existingWorkoutPlan.IsDeleted)
            {
                return new ApiResponse(400, "Workout Plan does not exist or is already deleted");
            }

            try
            {
                if (workoutPlanDto.Image != null)
                {
                    if (!string.IsNullOrEmpty(existingWorkoutPlan.ImageUrl))
                    {
                        await _imageService.DeleteImageAsync(existingWorkoutPlan.ImageUrl);
                    }

                    var fileResult = await _imageService.UploadImageAsync(workoutPlanDto.Image);
                    if (fileResult.Item1 == 1)
                    {
                        existingWorkoutPlan.ImageUrl = fileResult.Item2;
                    }
                    else
                    {
                        return new ApiResponse(400, fileResult.Item2);
                    }
                }
                else
                {
                    workoutPlanDto.ImageUrl = existingWorkoutPlan.ImageUrl;
                }

                existingWorkoutPlan.PlanName = workoutPlanDto.PlanName ?? existingWorkoutPlan.PlanName;
                existingWorkoutPlan.Description = workoutPlanDto.Description ?? existingWorkoutPlan.Description;
                await _context.SaveChangesAsync();

                return new ApiResponse(200, "Workout Plan updated successfully");
            }
            catch (Exception ex)
            {
                return new ApiResponse(500, "Error: " + ex.Message);
            }
        }

    }
}
