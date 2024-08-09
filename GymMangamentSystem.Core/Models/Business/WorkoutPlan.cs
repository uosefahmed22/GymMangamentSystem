using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangamentSystem.Core.Models.Business
{
    public class WorkoutPlan
    {
        public int WorkoutPlanId { get; set; }
        public string PlanName { get; set; }
        public string Description { get; set; }
        public string TrainerId { get; set; }
        public AppUser Trainer { get; set; }
        public ICollection<AppUserWorkoutPlan> MemberWorkoutPlans { get; set; }
        public ICollection<WorkoutExercise> WorkoutExercises { get; set; }
    }

}
