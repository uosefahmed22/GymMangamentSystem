using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangamentSystem.Core.Models.Business
{
    public class Feedback
    {
        public int FeedbackId { get; set; }
        public string? Comments { get; set; }
        public int Rating { get; set; }
        public bool IsDeleted { get; set; }
        public string UserId { get; set; }
        public AppUser User { get; set; }
    }


}
