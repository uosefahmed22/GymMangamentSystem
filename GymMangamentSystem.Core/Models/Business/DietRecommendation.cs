using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangamentSystem.Core.Models.Business
{
    public class DietRecommendation
    {
        public int DietRecommendationId { get; set; }
        public string Recommendation { get; set; }
        public ICollection<AppUser> Users { get; set; }
    }

}
