using AutoMapper;
using GymMangamentSystem.Core.Dtos.Business;
using GymMangamentSystem.Core.Errors;
using GymMangamentSystem.Core.IServices.Business;
using GymMangamentSystem.Core.Models.Business;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Crypto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymMangamentSystem.Reposatory.Data.Context;

namespace GymMangamentSystem.Reposatory.Services.Business
{
    public class AttendaceRepo : IAttendaceRepo
    {
        private readonly AppDBContext _context;
        private readonly IMapper _mapper;

        public AttendaceRepo(AppDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<ApiResponse> AddAttendance(AttendanceDto attendance)
        {
            try
            {
                var existingClass = await _context.Classes.FindAsync(attendance.ClassId);
                if (existingClass == null)
                {
                    return new ApiResponse(404, "Class not found");
                }
                var AtteendanceEntity = _mapper.Map<Attendance>(attendance);
                _context.Add(AtteendanceEntity);
                await _context.SaveChangesAsync();

                return new ApiResponse(200, "Attendance added successfully");
            }
            catch (Exception ex)
            {
                return new ApiResponse(400, "Error: " + ex.Message);
            }
        }
        public async Task<ApiResponse> DeleteAttendance(int id)
        {
            var ExsistingAttendance = await _context.Attendances.FindAsync(id);
            
            if (ExsistingAttendance == null || ExsistingAttendance.IsDeleted == true)
            {
                return new ApiResponse(404, "Attendance not found");
            }
            try
            {
                ExsistingAttendance.IsDeleted = true;

                _context.Entry(ExsistingAttendance).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return new ApiResponse(200, "Attendance deleted successfully");
            }
            catch (Exception ex)
            {
                return new ApiResponse(400, "Error: " + ex.Message);
            }

        }
        public async Task<IEnumerable<AttendanceDto>> GetAttendances()
        {
            try
            {
                var attendances = await _context.Attendances
                    .Where(x => x.IsDeleted == false).ToListAsync();

                var result = _mapper.Map<IEnumerable<AttendanceDto>>(attendances);

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
        }
    }
}
