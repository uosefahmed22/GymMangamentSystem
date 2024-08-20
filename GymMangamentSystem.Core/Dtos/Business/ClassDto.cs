using GymMangamentSystem.Core.Models.Business;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangamentSystem.Core.Dtos.Business
{
    public class ClassDto
    {
        public int? ClassId { get; set; }
        public string? ImageUrl { get; set; }
        [NotMapped]
        public IFormFile? Image { get; set; }
        public string ClassName { get; set; }
        public string Description { get; set; }
        public DateTime StartTime { get; set; }     
        public DateTime EndTime { get; set; }
        public string? TrainerId { get; set; }
    }
}
