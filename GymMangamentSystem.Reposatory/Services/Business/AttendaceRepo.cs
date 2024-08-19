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
                var UserCode = await _context.Users.FirstOrDefaultAsync(u => u.UserCode == attendance.UserCode);
                if (UserCode == null)
                {
                    return new ApiResponse(404, "User not found");
                }
                attendance.IsAttended = true;
                var newAttendance = _mapper.Map<Attendance>(attendance);
                await _context.Attendances.AddAsync(newAttendance);
                await _context.SaveChangesAsync();
                return new ApiResponse(200, "Attendance added successfully");
            }
            catch (Exception ex)
            {
                return new ApiResponse(400, "Error: " + ex.Message);
            }
        }
        public async Task<IEnumerable<object>> GetAttendancesForUser(string userCode)
        {
            try
            {
                var UserCode = await _context.Users.FirstOrDefaultAsync(u => u.UserCode == userCode);
                if (UserCode == null)
                {
                    return new List<object>();
                }
                var attendances = await _context.Attendances.Where(a => a.UserCode == userCode).ToListAsync();
                return attendances;
            }
            catch (Exception ex)
            {
                return new List<object>();
            }
        }
        public async Task<ApiResponse> DeleteAttendance(int id)
        {
            try
            {
                var attandanceId = await _context.Attendances.FirstOrDefaultAsync(a => a.AttendanceId == id);
                if (attandanceId == null)
                {
                    return new ApiResponse(404, "Attendance not found");
                }
                _context.Attendances.Remove(attandanceId);
                await _context.SaveChangesAsync();
                return new ApiResponse(200, "Attendance deleted successfully");
            }
            catch (Exception ex)
            {
                return new ApiResponse(400, "Error: " + ex.Message);
            }
        }
    }
}