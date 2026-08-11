using GymMangamentSystem.Core.IServices;
using GymMangamentSystem.Core.Enums.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangamentSystem.Core.Models.Business
{
    public class BMIRecord : ISoftDeletable
    {
        public int BMIRecordId { get; set; }
        public BMICategory Category { get; set; }
        public DateTime MeasurementDate { get; set; }=DateTime.Now;
        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string UserId { get; set; } = string.Empty;
        public AppUser User { get; set; } = null!;
        public decimal WeightInKg { get; set; }
        public decimal HeightInMeters { get; set; }
    }
    public static class BMIRecordExtensions
    {
        public static decimal CalculateBMI(this BMIRecord bmiRecord)
        {
            if (bmiRecord.HeightInMeters <= 0)
                throw new ArgumentException("Height must be greater than zero.");

            return bmiRecord.WeightInKg / (bmiRecord.HeightInMeters * bmiRecord.HeightInMeters);
        }

        public static BMICategory DetermineBMICategory(this decimal bmi)
        {   
            if (bmi < 18.5m) return BMICategory.Underweight;
            if (bmi < 25m) return BMICategory.Normal;
            if (bmi < 30m) return BMICategory.Overweight;
            if (bmi < 35m) return BMICategory.Obese;
            return BMICategory.SeverelyObese;
        }
    }
}
