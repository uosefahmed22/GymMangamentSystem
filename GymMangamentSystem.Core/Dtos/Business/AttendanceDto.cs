using GymMangamentSystem.Core.Models.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangamentSystem.Core.Dtos.Business
{
    public class AttendanceDto
    {
        public int? AttendanceId { get; set; }
        public bool IsAttended { get; set; }
        public string? UserCode { get; set; }
        public int ClassId { get; set; }
    }
}
