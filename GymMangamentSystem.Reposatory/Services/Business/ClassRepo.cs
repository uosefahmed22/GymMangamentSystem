using AutoMapper;
using GymMangamentSystem.Core.Dtos.Business;
using GymMangamentSystem.Core.Errors;
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
        private readonly IMapper _mapper;

        public ClassRepo(AppDBContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<ApiResponse> AddClass(ClassDto classDto)
        {
            try
            {
                var AtteendanceEntity = _mapper.Map<Class>(classDto);
                _context.Add(AtteendanceEntity);
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
            var ExsistingClass = await _context.Classes.FindAsync(id);
            if (ExsistingClass == null)
            {
                return new ApiResponse(404, "Class not found");
            }
            try
            {
                _context.Classes.Remove(ExsistingClass);
                _context.Update(ExsistingClass);
                await _context.SaveChangesAsync();
                return new ApiResponse(200, "Class deleted successfully");
            }
            catch (Exception ex)
            {
                return new ApiResponse(400, "Error: " + ex.Message);
            }
        }
        public async Task<ClassDto> GetClass(int id)
        {
            try
            {
                var Class = await _context.Classes.FindAsync(id);
                if (Class == null)
                {
                    throw null;
                }
                var ClassDto = _mapper.Map<ClassDto>(Class);
                return ClassDto;
            }
            catch (Exception ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
        }
        public async Task<IEnumerable<ClassDto>> GetClasses()
        {
            try
            {
                var Classes = await _context.Classes
                    .Include(a => a.Trainer)
                    .Include(a => a.Attendances)
                    .Include(a => a.Memberships)
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
                if (existingClass == null)
                {
                    return new ApiResponse(404, "Class not found");
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
