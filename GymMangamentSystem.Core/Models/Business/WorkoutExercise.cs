using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangamentSystem.Core.Models.Business
{
    public class WorkoutExercise
    {
        public int WorkoutExerciseId { get; set; }
        public int Repetitions { get; set; }
        public int Sets { get; set; }
        public int WorkoutPlanId { get; set; }
        public WorkoutPlan WorkoutPlan { get; set; }
        public int ExerciseId { get; set; }
        public Exercise Exercise { get; set; }
    }

}
