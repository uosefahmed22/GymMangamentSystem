using GymMangamentSystem.Core.Dtos.Business;
using GymMangamentSystem.Core.Enums.Business;
using GymMangamentSystem.Core.IServices;
using GymMangamentSystem.Core.Models.Business;

namespace GymMangamentSystem.Reposatory.Services;

public sealed class AppMapper : IAppMapper
{
    public TDestination Map<TDestination>(object source)
    {
        ArgumentNullException.ThrowIfNull(source);

        return source switch
        {
            AttendanceDto dto when Is<Attendance, TDestination>() => Cast<TDestination>(new Attendance { AttendanceId = dto.AttendanceId ?? 0, IsAttended = dto.IsAttended, UserCode = dto.UserCode ?? string.Empty, ClassId = dto.ClassId }),
            Attendance entity when Is<AttendanceDto, TDestination>() => Cast<TDestination>(new AttendanceDto { AttendanceId = entity.AttendanceId, IsAttended = entity.IsAttended, UserCode = entity.UserCode, ClassId = entity.ClassId }),
            BMIRecordDto dto when Is<BMIRecord, TDestination>() => Cast<TDestination>(new BMIRecord { BMIRecordId = dto.BMIRecordId ?? 0, Category = (BMICategory)(dto.Category ?? 0), UserId = dto.UserId ?? string.Empty, WeightInKg = dto.WeightInKg, HeightInMeters = dto.HeightInMeters }),
            BMIRecord entity when Is<BMIRecordDto, TDestination>() => Cast<TDestination>(new BMIRecordDto { BMIRecordId = entity.BMIRecordId, Category = (int)entity.Category, UserId = entity.UserId, WeightInKg = entity.WeightInKg, HeightInMeters = entity.HeightInMeters }),
            ClassDto dto when Is<Class, TDestination>() => Cast<TDestination>(new Class
            {
                ClassId = dto.ClassId ?? 0, ImageUrl = dto.ImageUrl, Image = dto.Image!, ClassName = dto.ClassName,
                Description = dto.Description, StartTime = dto.StartTime, EndTime = dto.EndTime, TrainerId = dto.TrainerId ?? string.Empty
            }),
            Class entity when Is<ClassDto, TDestination>() => Cast<TDestination>(new ClassDto
            {
                ClassId = entity.ClassId, ImageUrl = entity.ImageUrl, Image = entity.Image, ClassName = entity.ClassName,
                Description = entity.Description, StartTime = entity.StartTime, EndTime = entity.EndTime, TrainerId = entity.TrainerId
            }),
            ExerciseCategoryDto dto when Is<ExerciseCategory, TDestination>() => Cast<TDestination>(new ExerciseCategory
            {
                ExerciseCategoryId = dto.ExerciseCategoryId ?? 0, ImageUrl = dto.ImageUrl, Image = dto.Image, CategoryName = dto.CategoryName
            }),
            ExerciseCategory entity when Is<ExerciseCategoryDto, TDestination>() => Cast<TDestination>(new ExerciseCategoryDto
            {
                ExerciseCategoryId = entity.ExerciseCategoryId, ImageUrl = entity.ImageUrl, Image = entity.Image, CategoryName = entity.CategoryName
            }),
            ExerciseDto dto when Is<Exercise, TDestination>() => Cast<TDestination>(new Exercise
            {
                ExerciseId = dto.ExerciseId ?? 0, ExerciseName = dto.ExerciseName, Description = dto.Description,
                ImageUrl = dto.ImageUrl, Image = dto.Image, Repetitions = dto.Repetitions, Sets = dto.Sets,
                WorkoutPlanId = dto.WorkoutPlanId, ExerciseCategoryId = dto.ExerciseCategoryId
            }),
            Exercise entity when Is<ExerciseDto, TDestination>() => Cast<TDestination>(new ExerciseDto
            {
                ExerciseId = entity.ExerciseId, ExerciseName = entity.ExerciseName, Description = entity.Description,
                ImageUrl = entity.ImageUrl, Image = entity.Image, Repetitions = entity.Repetitions, Sets = entity.Sets,
                WorkoutPlanId = entity.WorkoutPlanId, ExerciseCategoryId = entity.ExerciseCategoryId
            }),
            FeedbackDto dto when Is<Feedback, TDestination>() => Cast<TDestination>(new Feedback
            {
                FeedbackId = dto.FeedbackId ?? 0, Comments = dto.Comments, Rating = dto.Rating,
                UserId = dto.UserId ?? string.Empty, TrainerId = dto.TrainerId ?? string.Empty
            }),
            Feedback entity when Is<FeedbackDto, TDestination>() => Cast<TDestination>(new FeedbackDto
            {
                FeedbackId = entity.FeedbackId, Comments = entity.Comments, Rating = entity.Rating, UserId = entity.UserId, TrainerId = entity.TrainerId
            }),
            MealDto dto when Is<Meal, TDestination>() => Cast<TDestination>(new Meal
            {
                MealId = dto.MealId, ImageUrl = dto.ImageUrl, Image = dto.Image!, MealName = dto.MealName,
                Description = dto.Description, NutritionPlanId = dto.NutritionPlanId, MealsCategoryId = dto.MealsCategoryId
            }),
            Meal entity when Is<MealDto, TDestination>() => Cast<TDestination>(new MealDto
            {
                MealId = entity.MealId, ImageUrl = entity.ImageUrl, Image = entity.Image, MealName = entity.MealName,
                Description = entity.Description, NutritionPlanId = entity.NutritionPlanId, MealsCategoryId = entity.MealsCategoryId
            }),
            MealsCategoryDto dto when Is<MealsCategory, TDestination>() => Cast<TDestination>(new MealsCategory
            {
                MealsCategoryId = dto.MealsCategoryId, ImageUrl = dto.ImageUrl, Image = dto.Image!, CategoryName = dto.CategoryName
            }),
            MealsCategory entity when Is<MealsCategoryDto, TDestination>() => Cast<TDestination>(new MealsCategoryDto
            {
                MealsCategoryId = entity.MealsCategoryId, ImageUrl = entity.ImageUrl, Image = entity.Image, CategoryName = entity.CategoryName
            }),
            MembershipDto dto when Is<Membership, TDestination>() => Cast<TDestination>(new Membership
            {
                MembershipId = dto.MembershipId ?? 0, ImageUrl = dto.ImageUrl, Image = dto.Image!,
                MembershipType = (MembershipType)dto.MembershipType, StartDate = dto.StartDate, EndDate = dto.EndDate,
                Price = dto.Price, ClassId = dto.ClassId
            }),
            Membership entity when Is<MembershipDto, TDestination>() => Cast<TDestination>(new MembershipDto
            {
                MembershipId = entity.MembershipId, ImageUrl = entity.ImageUrl, Image = entity.Image,
                MembershipType = (int)entity.MembershipType, StartDate = entity.StartDate, EndDate = entity.EndDate,
                Price = entity.Price, UserId = entity.User?.Id, ClassId = entity.ClassId
            }),
            NotificationDto dto when Is<Notification, TDestination>() => Cast<TDestination>(new Notification
            {
                NotificationId = dto.NotificationId ?? 0, Message = dto.Message, Date = dto.Date ?? DateTime.UtcNow
            }),
            Notification entity when Is<NotificationDto, TDestination>() => Cast<TDestination>(new NotificationDto
            {
                NotificationId = entity.NotificationId, Message = entity.Message, Date = entity.Date
            }),
            NutritionPlanDto dto when Is<NutritionPlan, TDestination>() => Cast<TDestination>(new NutritionPlan
            {
                NutritionPlanId = dto.NutritionPlanId, ImageUrl = dto.ImageUrl, Image = dto.Image!, PlanName = dto.PlanName, Description = dto.Description
            }),
            NutritionPlan entity when Is<NutritionPlanDto, TDestination>() => Cast<TDestination>(new NutritionPlanDto
            {
                NutritionPlanId = entity.NutritionPlanId, ImageUrl = entity.ImageUrl, Image = entity.Image, PlanName = entity.PlanName, Description = entity.Description
            }),
            WorkoutPlanDto dto when Is<WorkoutPlan, TDestination>() => Cast<TDestination>(new WorkoutPlan
            {
                WorkoutPlanId = dto.WorkoutPlanId ?? 0, ImageUrl = dto.ImageUrl, Image = dto.Image,
                PlanName = dto.PlanName, Description = dto.Description, TrainerId = dto.TrainerId
            }),
            WorkoutPlan entity when Is<WorkoutPlanDto, TDestination>() => Cast<TDestination>(new WorkoutPlanDto
            {
                WorkoutPlanId = entity.WorkoutPlanId, ImageUrl = entity.ImageUrl, Image = entity.Image,
                PlanName = entity.PlanName, Description = entity.Description, TrainerId = entity.TrainerId
            }),
            IEnumerable<Class> items when IsCollectionOf<ClassDto, TDestination>() => Collection<TDestination, ClassDto>(items.Select(Map<ClassDto>)),
            IEnumerable<ExerciseCategory> items when IsCollectionOf<ExerciseCategoryDto, TDestination>() => Collection<TDestination, ExerciseCategoryDto>(items.Select(Map<ExerciseCategoryDto>)),
            IEnumerable<Exercise> items when IsCollectionOf<ExerciseDto, TDestination>() => Collection<TDestination, ExerciseDto>(items.Select(Map<ExerciseDto>)),
            IEnumerable<Feedback> items when IsCollectionOf<FeedbackDto, TDestination>() => Collection<TDestination, FeedbackDto>(items.Select(Map<FeedbackDto>)),
            IEnumerable<Meal> items when IsCollectionOf<MealDto, TDestination>() => Collection<TDestination, MealDto>(items.Select(Map<MealDto>)),
            IEnumerable<MealsCategory> items when IsCollectionOf<MealsCategoryDto, TDestination>() => Collection<TDestination, MealsCategoryDto>(items.Select(Map<MealsCategoryDto>)),
            IEnumerable<Membership> items when IsCollectionOf<MembershipDto, TDestination>() => Collection<TDestination, MembershipDto>(items.Select(Map<MembershipDto>)),
            IEnumerable<Notification> items when IsCollectionOf<NotificationDto, TDestination>() => Collection<TDestination, NotificationDto>(items.Select(Map<NotificationDto>)),
            IEnumerable<NutritionPlan> items when IsCollectionOf<NutritionPlanDto, TDestination>() => Collection<TDestination, NutritionPlanDto>(items.Select(Map<NutritionPlanDto>)),
            IEnumerable<WorkoutPlan> items when IsCollectionOf<WorkoutPlanDto, TDestination>() => Collection<TDestination, WorkoutPlanDto>(items.Select(Map<WorkoutPlanDto>)),
            _ => throw new InvalidOperationException($"No mapping is configured from {source.GetType().Name} to {typeof(TDestination).Name}.")
        };
    }

    private static bool Is<TExpected, TDestination>() => typeof(TDestination) == typeof(TExpected);
    private static bool IsCollectionOf<TItem, TDestination>() => typeof(TDestination) == typeof(IEnumerable<TItem>) || typeof(TDestination) == typeof(List<TItem>);
    private static TDestination Collection<TDestination, TItem>(IEnumerable<TItem> items) => Cast<TDestination>(items.ToList());
    private static TDestination Cast<TDestination>(object value) => (TDestination)value;
}
