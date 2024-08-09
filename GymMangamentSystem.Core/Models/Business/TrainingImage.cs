using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangamentSystem.Core.Models.Business
{
    public class TrainingImage
    {
        public int TrainingImageId { get; set; }
        public string ImageUrl { get; set; }
        public int ExerciseId { get; set; }
        public Exercise Exercise { get; set; }
    }

}
