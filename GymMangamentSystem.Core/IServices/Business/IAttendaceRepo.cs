﻿using GymMangamentSystem.Core.Dtos.Business;
using GymMangamentSystem.Core.Errors;
using GymMangamentSystem.Core.Models.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangamentSystem.Core.IServices.Business
{
    public interface IAttendaceRepo
    {
        Task<ApiResponse> AddAttendance(AttendanceDto attendance);
        Task<ApiResponse> DeleteAttendance(int id);
        Task<IEnumerable<object>> GetAttendancesForUser(string userId);
    }
}
