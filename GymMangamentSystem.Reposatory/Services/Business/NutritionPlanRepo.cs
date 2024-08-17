using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using GymMangamentSystem.Core.Dtos.Business;
using GymMangamentSystem.Core.Errors;
using GymMangamentSystem.Core.IServices;
using GymMangamentSystem.Core.IServices.Business;
using GymMangamentSystem.Core.Models.Business;
using GymMangamentSystem.Reposatory.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace GymMangamentSystem.Reposatory.Services.Business
{
    public class NutritionPlanRepo : INutritionPlanRepo
    {
        private readonly AppDBContext _context;
        private readonly IMapper _mapper;
        private readonly IImageService _imageService;

        public NutritionPlanRepo(AppDBContext context, IMapper mapper, IImageService fileService)
        {
            _context = context;
            _mapper = mapper;
            _imageService = fileService;
        }
        public async Task<ApiResponse> CreateNutritionPlan(NutritionPlanDto nutritionPlanDto)
        {
            var exsistingNutritionPlan = await _context.NutritionPlans.FirstOrDefaultAsync(x => x.PlanName == nutritionPlanDto.PlanName);
            if (exsistingNutritionPlan != null)
            {
                return new ApiResponse(400, "Nutrition Plan already exists");
            }
            try
            {
                if (nutritionPlanDto.Image != null)
                {
                    var fileResult = await _imageService.UploadImageAsync(nutritionPlanDto.Image);
                    if (fileResult.Item1 == 1)
                    {
                        nutritionPlanDto.ImageUrl = fileResult.Item2;
                    }
                    else
                    {
                        return new ApiResponse(400, fileResult.Item2);
                    }
                }
                var mappedNutritionPlan = _mapper.Map<NutritionPlan>(nutritionPlanDto);
                _context.NutritionPlans.Add(mappedNutritionPlan);
                await _context.SaveChangesAsync();
                return new ApiResponse(200, "Nutrition Plan added successfully");
            }
            catch (Exception ex)
            {
                return new ApiResponse(500, "Error: " + ex.Message);
            }
        }
        public async Task<ApiResponse> DeleteNutritionPlan(int nutritionPlanId)
        {
            var nutritionPlan = await _context.NutritionPlans.FirstOrDefaultAsync(x => x.NutritionPlanId == nutritionPlanId);
            if (nutritionPlan == null|| nutritionPlan.IsDeleted)
            {
                return new ApiResponse(404, "Nutrition Plan not found");
            }
            try
            {
                nutritionPlan.IsDeleted = true;
                await _context.SaveChangesAsync();
                return new ApiResponse(200, "Nutrition Plan deleted successfully");
            }
            catch (Exception ex)
            {
                return new ApiResponse(500, "Error: " + ex.Message);
            }
        }
        public async Task<NutritionPlanDto> GetNutritionPlan(int nutritionPlanId)
        {
            var nutritionPlan = await _context.NutritionPlans.FirstOrDefaultAsync(x => x.NutritionPlanId == nutritionPlanId);
            try
            {
                if (nutritionPlan == null || nutritionPlan.IsDeleted)
                {
                    return null;
                }

                var nutritionPlanDto = _mapper.Map<NutritionPlanDto>(nutritionPlan);
                return nutritionPlanDto;
            }
            catch (Exception ex) {
                throw new Exception("Error: " + ex.Message);
            }
        }
        public async Task<IEnumerable<NutritionPlanDto>> GetNutritionPlans()
        {
            var nutritionPlans = await _context.NutritionPlans.Where(x => !x.IsDeleted).ToListAsync();
            try
            {
                var nutritionPlansDto = _mapper.Map<IEnumerable<NutritionPlanDto>>(nutritionPlans);
                return nutritionPlansDto;
            }
            catch (Exception ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
        }
        public async Task<ApiResponse> UpdateNutritionPlan(int nutritionPlanId, NutritionPlanDto nutritionPlanDto)
        {
            var nutritionPlan = await _context.NutritionPlans.FirstOrDefaultAsync(x => x.NutritionPlanId == nutritionPlanId);
            if (nutritionPlan == null)
            {
                return new ApiResponse(404, "Nutrition Plan not found");
            }
            try
            {
                if (nutritionPlanDto.Image != null)
                {
                    if (!string.IsNullOrEmpty(nutritionPlan.ImageUrl))
                    {
                        await _imageService.DeleteImageAsync(nutritionPlan.ImageUrl);
                    }
                    var fileResult = await _imageService.UploadImageAsync(nutritionPlanDto.Image);
                    if (fileResult.Item1 == 1)
                    {
                        nutritionPlanDto.ImageUrl = fileResult.Item2;
                    }
                    else
                    {
                        return new ApiResponse(400, fileResult.Item2);
                    }
                }
                nutritionPlan.PlanName = nutritionPlanDto.PlanName;
                nutritionPlan.Description = nutritionPlanDto.Description;
                nutritionPlan.ImageUrl = nutritionPlanDto.ImageUrl;

                await _context.SaveChangesAsync();
                return new ApiResponse(200, "Nutrition Plan updated successfully");
            }
            catch (Exception ex)
            {
                return new ApiResponse(500, "Error: " + ex.Message);
            }
        }
    }
}
