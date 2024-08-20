using GymMangamentSystem.Core.Models.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangamentSystem.Core.Dtos.Business
{
    public class FeedbackDto
    {
        public int? FeedbackId { get; set; }
        public string? Comments { get; set; }
        public int Rating { get; set; }
        public string? UserId { get; set; }
        public string? TrainerId { get; set; }
    }
}
