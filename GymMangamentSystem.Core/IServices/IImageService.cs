using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangamentSystem.Core.IServices
{
    public interface IImageService
    {
        Task DeleteImageAsync(string imageFileName);
        Task<Tuple<int, string>> UploadImageAsync(IFormFile imageFile);
    }
}
