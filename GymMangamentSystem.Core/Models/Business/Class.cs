using GymMangamentSystem.Core.IServices;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangamentSystem.Core.Models.Business
{
    public class Class : ISoftDeletable
    {
        public int ClassId { get; set; }
        public string? ImageUrl { get; set; }
        [NotMapped]
        public IFormFile? Image { get; set; }
        public string ClassName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string TrainerId { get; set; } = string.Empty;
        public AppUser Trainer { get; set; } = null!;
        public ICollection<Membership> Memberships { get; set; } = [];
        public ICollection<Attendance> Attendances { get; set; } = [];
    }
}
