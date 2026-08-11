using GymMangamentSystem.Core.Dtos.Business;
using GymMangamentSystem.Core.Errors;
using GymMangamentSystem.Core.Models.Common;
using GymMangamentSystem.Core.Models.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangamentSystem.Core.IServices.Business
{
    public interface IAttendanceRepo
    {
        Task<ApiResponse> AddAttendance(AttendanceDto attendance);
        Task<IEnumerable<object>> GetAttendancesForUser(string userCode, PaginationParameters? pagination = null);
        Task<ApiResponse> DeleteAttendance(int id);
    }
}
