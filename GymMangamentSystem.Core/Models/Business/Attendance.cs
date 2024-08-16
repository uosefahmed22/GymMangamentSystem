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
        public DateTime AttendanceDate { get; set; } = DateTime.Now;
        public bool IsDeleted { get; set; }
        public string UserId { get; set; }
        public AppUser User { get; set; }
        public int ClassId { get; set; }
        public Class Class { get; set; }
    }
}
