using GymMangamentSystem.Core.Dtos.Business;
using GymMangamentSystem.Core.Errors;
using GymMangamentSystem.Core.Models.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangamentSystem.Core.IServices.Business
{
    public interface IBMIRecordRepo
    {
        Task<IEnumerable<object>> GetBMIRecordsForUser(string userId);
        Task<object> GetBMIRecordById(int id);
        Task<ApiResponse> AddBMIRecord(BMIRecordDto bmiRecord);
        Task<ApiResponse> DeleteBMIRecord(int id);
    }
}
