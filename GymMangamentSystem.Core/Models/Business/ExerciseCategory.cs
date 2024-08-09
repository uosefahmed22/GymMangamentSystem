using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangamentSystem.Core.Models.Business
{
    public class ExerciseCategory
    {
        public int ExerciseCategoryId { get; set; }
        public string CategoryName { get; set; }
        public ICollection<Exercise> Exercises { get; set; }
    }


}
