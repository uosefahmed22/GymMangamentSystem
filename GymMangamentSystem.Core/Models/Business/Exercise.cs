using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangamentSystem.Core.Models.Business
{

    public class Exercise
    {
        public int ExerciseId { get; set; }
        public string ExerciseName { get; set; }
        public string Description { get; set; }
        public string? ImageUrl { get; set; }
        [NotMapped]
        public IFormFile Image { get; set; }
        public int Repetitions { get; set; }
        public int Sets { get; set; }
        public bool IsDeleted { get; set; }
        public int? WorkoutPlanId { get; set; }
        public WorkoutPlan? WorkoutPlan { get; set; }
        public int ExerciseCategoryId { get; set; }
        public ExerciseCategory ExerciseCategory { get; set; }
    }
}