using GymMangamentSystem.Core.Models.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangamentSystem.Core.Dtos.Business
{
    public class EnrollmentDto
    {
        public int EnrollmentId { get; set; }
        public DateTime EnrollmentDate { get; set; }=DateTime.Now;
        public string UserId { get; set; }
        public AppUser User { get; set; }
        public int ClassId { get; set; }
        public Class Class { get; set; }
    }
}
