using AutoMapper;
using GymMangamentSystem.Core.Dtos.Business;
using GymMangamentSystem.Core.Models.Business;

namespace GymMangamentSystem.Apis.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {

            CreateMap<Attendance,AttendanceDto>().ReverseMap();
            CreateMap<Class,ClassDto>().ReverseMap();
            CreateMap<ExerciseCategory,ExerciseCategoryDto>().ReverseMap();
            CreateMap<WorkoutPlan,WorkoutPlanDto>().ReverseMap();
            CreateMap<Exercise,ExerciseDto>().ReverseMap();
        }
    }
}
