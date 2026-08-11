using GymMangamentSystem.Core.Models.Common;
using GymMangamentSystem.Reposatory.Data;
using GymMangamentSystem.Core.IServices;
using GymMangamentSystem.Core.Dtos.Business;
using GymMangamentSystem.Core.Enums.Business;
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
    public class BMIRecordRepo : IBMIRecordRepo
    {
        private readonly AppDBContext _context;
        private readonly IAppMapper _mapper;

        public BMIRecordRepo(AppDBContext context, IAppMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<ApiResponse> AddBMIRecord(BMIRecordDto bmiRecordDto)
        {
            try
            {
                if (bmiRecordDto.WeightInKg <= 0 || bmiRecordDto.HeightInMeters <= 0)
                {
                    return new ApiResponse(400, "Weight and height must be greater than 0.");
                }

                var bmiRecord = _mapper.Map<BMIRecord>(bmiRecordDto);

                var bmiValue = bmiRecord.CalculateBMI();
                bmiRecord.Category = bmiValue.DetermineBMICategory();

                await _context.AddAsync(bmiRecord);
                await _context.SaveChangesAsync();
                return new ApiResponse(200, "BMI record added successfully", bmiRecord.Category.ToString());
            }
            catch (Exception ex)
            {
                return new ApiResponse(400, "Error: " + ex.Message);
            }
        }
        public async Task<ApiResponse> DeleteBMIRecord(int id)
        {
            var existingBMIRecord = await _context.BMIRecords.FindAsync(id);
            if (existingBMIRecord == null || existingBMIRecord.IsDeleted == true)
            {
                return new ApiResponse(404, "BMI record not found");
            }
            try
            {
                _context.BMIRecords.Remove(existingBMIRecord);
                await _context.SaveChangesAsync();
                return new ApiResponse(200, "BMI record deleted successfully");
            }
            catch (Exception ex)
            {
                return new ApiResponse(400, "Error: " + ex.Message);
            }
        }
        public async Task<IEnumerable<object>> GetBMIRecordsForUser(string userId, PaginationParameters? pagination = null)
        {
            try
            {
                var bmiRecords = await _context.BMIRecords
                    .AsNoTracking()
                    .Where(x => x.UserId == userId)
                    .OrderByDescending(x => x.MeasurementDate)
                    .ApplyPagination(pagination)
                    .Select(x => new
                    {
                        id = x.BMIRecordId,
                        Category = x.Category.ToString(),
                        x.MeasurementDate
                    })
                    .ToListAsync();

                return bmiRecords;
            }
            catch (Exception ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
        }
    }
}
