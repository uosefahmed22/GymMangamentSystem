using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangamentSystem.Core.Models.Business
{
    public class Attendance
    {
        public int AttendanceId { get; set; }
        public bool IsAttended { get; set; }
        public DateTime AttendanceDate { get; set; } = DateTime.UtcNow;
        public string UserCode { get; set; } = string.Empty;
        public int ClassId { get; set; }
        public Class Class { get; set; } = null!;
    }
}
