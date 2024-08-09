using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangamentSystem.Core.Models.Business
{
    public class Membership
    {
        public int MembershipId { get; set; }
        public string MembershipType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal Price { get; set; }
        public string UserId { get; set; }
        public AppUser User { get; set; }
        public ICollection<Payment> Payments { get; set; }
    }
}
