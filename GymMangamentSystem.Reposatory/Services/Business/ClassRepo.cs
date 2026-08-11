using GymMangamentSystem.Core.Models.Common;
using GymMangamentSystem.Reposatory.Data;
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
    public class ClassRepo : IClassRepo
    {
        private readonly AppDBContext _context;
        private readonly IAppMapper _mapper;
        private readonly IImageService _imageService;

        public ClassRepo(AppDBContext context,IAppMapper mapper,IImageService imageService)
        {
            _context = context;
            _mapper = mapper;
            _imageService = imageService;
        }
        public async Task<ApiResponse> AddClass(ClassDto classDto)
        {
            try
            {
                var existingClass =await _context.Classes.FirstOrDefaultAsync(x => x.ClassName == classDto.ClassName);
                if (classDto == null || existingClass != null)
                {
                    return new ApiResponse(400, "Class is null or already exists");
                }
                if (classDto.Image != null)
                {
                    var fileResult = await _imageService.UploadImageAsync(classDto.Image);
                    if (fileResult.Item1 == 1)
                    {
                        classDto.ImageUrl = fileResult.Item2;
                    }
                    else
                    {
                        return new ApiResponse(400, fileResult.Item2);
                    }
                }
                var classEntity = _mapper.Map<Class>(classDto);
                await _context.AddAsync(classEntity);
                await _context.SaveChangesAsync();

                return new ApiResponse(200, "Class added successfully");
            }
            catch (Exception ex)
            {
                return new ApiResponse(400, "Error: " + ex.Message);
            }
        }
        public async Task<ApiResponse> DeleteClass(int id)
        {
            var existingClass = await _context.Classes.FindAsync(id);
            if (existingClass == null || existingClass.IsDeleted==true)
            {
                return new ApiResponse(404, "Class not found");
            }
            try
            {
                _context.Classes.Remove(existingClass);
                await _context.SaveChangesAsync();
                return new ApiResponse(200, "Class deleted successfully");
            }
            catch (Exception ex)
            {
                return new ApiResponse(400, "Error: " + ex.Message);
            }
        }
        public async Task<ClassDto?> GetClass(int id)
        {
            try
            {
                var Class = await _context.Classes.FindAsync(id);
                if (Class == null || Class.IsDeleted == true)
                {
                    return null;
                }

                var ClassDto = _mapper.Map<ClassDto>(Class);
                return ClassDto;
            }
            catch (Exception ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
        }
        public async Task<IEnumerable<ClassDto>> GetClasses(PaginationParameters? pagination = null)
        {
            try
            {
                var Classes = await _context.Classes
                    .AsNoTracking()
                    .OrderBy(x => x.ClassId)
                    .ApplyPagination(pagination)
                    .ToListAsync();
                var ClassesDto = _mapper.Map<IEnumerable<ClassDto>>(Classes);
                return ClassesDto;
            }
            catch (Exception ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
        }
        public async Task<ApiResponse> UpdateClass(int id, ClassDto classDto)
        {
            try
            {
                var existingClass = await _context.Classes.FindAsync(id);
                if (existingClass == null || existingClass.IsDeleted == true)
                {
                    return new ApiResponse(404, "Class not found");
                }
                if (classDto.Image != null)
                {
                    if(!string.IsNullOrEmpty(existingClass.ImageUrl))
                    {
                       await _imageService.DeleteImageAsync(existingClass.ImageUrl);
                    }
                    var fileResult = await _imageService.UploadImageAsync(classDto.Image);
                    if (fileResult.Item1 == 1)
                    {
                        classDto.ImageUrl = fileResult.Item2;
                    }
                    else
                    {
                        return new ApiResponse(400, fileResult.Item2);
                    }
                }

                existingClass.ClassName = classDto.ClassName;
                existingClass.Description = classDto.Description;
                existingClass.StartTime = classDto.StartTime;
                existingClass.EndTime = classDto.EndTime;
                existingClass.ImageUrl = classDto.ImageUrl;

                _context.Update(existingClass);
                await _context.SaveChangesAsync();

                return new ApiResponse(200, "Class updated successfully");
            }
            catch (Exception ex)
            {
                return new ApiResponse(400, "Error: " + ex.Message);
            }
        }
    }
}
