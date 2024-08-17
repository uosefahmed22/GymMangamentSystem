using GymMangamentSystem.Apis.Helpers;
using GymMangamentSystem.Core.Errors;
using GymMangamentSystem.Core.IServices;
using GymMangamentSystem.Core.IServices.Business;
using GymMangamentSystem.Reposatory.Data.Context;
using GymMangamentSystem.Reposatory.Services.Business;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace GymMangamentSystem.Apis.Extention
{
    public static class ApplictionServiceExtention
    {
        public static IServiceCollection AddAplictionService(this IServiceCollection service)
        {
            service.Configure<ApiBehaviorOptions>(Options =>
            {
                Options.InvalidModelStateResponseFactory = (actionContext) =>
                {
                    var Errors = actionContext.ModelState
                        .Where(P => P.Value.Errors.Count() > 0)
                        .SelectMany(P => P.Value.Errors)
                        .Select(E => E.ErrorMessage)
                        .ToArray();

                    var ValidationErrorResponse = new ApiValidationErrorResponse()
                    {
                        Errors = Errors
                    };

                    return new BadRequestObjectResult(ValidationErrorResponse);
                };
            });


            service.AddAutoMapper(typeof(MappingProfile));
            service.AddScoped<IClassRepo, ClassRepo>();
            service.AddScoped<IAttendaceRepo, AttendaceRepo>();
            service.AddScoped<IExerciseCategoryRepo, ExerciseCategoryRepo>();
            service.AddScoped<IImageService, ImageService>();
            service.AddScoped<IWorkoutPlanRepo, WorkoutPlanRepo>();
            service.AddScoped<IExerciseRepo, ExerciseRepo>();
            service.AddScoped<IFeedbackRepo, FeedbackRepo>();
            service.AddScoped<IBMIRecordRepo, BMIRecordRepo>();


            return service;
        }
    }
}
