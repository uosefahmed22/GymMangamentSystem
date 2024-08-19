using GymMangamentSystem.Core.Dtos.Business;
using GymMangamentSystem.Core.Errors;
using GymMangamentSystem.Core.IServices;
using GymMangamentSystem.Core.IServices.Auth;
using GymMangamentSystem.Core.Models.Business;
using GymMangamentSystem.Core.Models.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangamentSystem.Reposatory.Services.Auth
{
    public class ProfileRepo : IProfileRepo
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IImageService _imageService;

        public ProfileRepo(UserManager<AppUser> userManager, IImageService imageService)
        {
            _userManager = userManager;
            _imageService = imageService;
        }
        public async Task<ApiResponse> DeleteAccount(string userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    return new ApiResponse(404, "User not found");
                }
                user.IsDeleted = true;
                user.DeletedAt = DateTime.Now;
                await _userManager.UpdateAsync(user);
                return new ApiResponse(200, "Account deleted successfully");
            }
            catch (Exception e)
            {
                return new ApiResponse(500, e.Message);
            }
        }
        public Task<MealDto> GetFavoriteMeals(string userId)
        {
            throw new NotImplementedException();
        }
        public async Task<ApiResponse> UpdateName(string userId, string name)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    return new ApiResponse(404, "User not found");
                }
                user.DisplayName = name;
                await _userManager.UpdateAsync(user);
                return new ApiResponse(200, "Name updated successfully");
            }
            catch (Exception e)
            {
                return new ApiResponse(500, e.Message);
            }
        }
        public async Task<ApiResponse> UpdateUserImageAsync(string userId, IFormFile? image, string? imageUrl)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    return new ApiResponse(404, "User not found");
                }
                if (image == null)
                {
                    if (!string.IsNullOrEmpty(imageUrl))
                    {
                        await _imageService.DeleteImageAsync(imageUrl);
                        user.ProfileImageName = null;
                    }
                }
                else
                {
                    var result = await _imageService.UploadImageAsync(image);
                    if (result.Item1 == 0)
                    {
                        return new ApiResponse(400, result.Item2);
                    }
                    if (!string.IsNullOrEmpty(imageUrl))
                    {
                        await _imageService.DeleteImageAsync(imageUrl);
                    }
                    user.ProfileImageName = result.Item2;
                }
                await _userManager.UpdateAsync(user);
                return new ApiResponse(200, "Image updated successfully");
            }
            catch (Exception e)
            {
                return new ApiResponse(500, e.Message);
            }
        }
    }
}
                    