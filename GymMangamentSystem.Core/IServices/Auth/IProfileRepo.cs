using GymMangamentSystem.Core.Dtos.Business;
using GymMangamentSystem.Core.Errors;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangamentSystem.Core.IServices.Auth
{
    public interface IProfileRepo
    {
        Task<ApiResponse> DeleteAccount(string userId);
        Task<ApiResponse> UpdateUserImageAsync(string userId, IFormFile? image, string imageUrl);
        Task<ApiResponse> UpdateName(string userId, string name);
        Task<MealDto> GetFavoriteMeals(string userId);

    }
}
