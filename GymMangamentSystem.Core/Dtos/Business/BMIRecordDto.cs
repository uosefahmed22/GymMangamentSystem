using GymMangamentSystem.Core.Enums.Business;
using GymMangamentSystem.Core.Models.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangamentSystem.Core.Dtos.Business
{
    public class BMIRecordDto
    {
        public int? BMIRecordId { get; set; }
        public int? Category { get; set; }
        public string? UserId { get; set; }
        public decimal WeightInKg { get; set; }
        public decimal HeightInMeters { get; set; }
    }
}
