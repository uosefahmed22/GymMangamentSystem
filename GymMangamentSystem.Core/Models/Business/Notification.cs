using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangamentSystem.Core.Models.Business
{
    public class Notification
    {
        public int NotificationId { get; set; }
        public string Message { get; set; }
        public bool IsRead { get; set; }
        public DateTime Date { get; set; }
        public bool IsDeleted { get; set; }
        public string? UserId { get; set; }
        public AppUser? User { get; set; }
    }
}
