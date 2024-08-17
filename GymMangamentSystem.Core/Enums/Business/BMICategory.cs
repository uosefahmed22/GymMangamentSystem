using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangamentSystem.Core.Enums.Business
{
    public enum BMICategory
    {
        Underweight,  // BMI < 18.5
        Normal,       // BMI 18.5 - 24.9
        Overweight,   // BMI 25 - 29.9
        Obese,        // BMI 30 - 34.9
        SeverelyObese // BMI >= 35
    }
}
