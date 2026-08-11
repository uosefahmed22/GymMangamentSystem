using GymMangamentSystem.Core.Dtos.Business;
using GymMangamentSystem.Core.Enums.Business;
using GymMangamentSystem.Core.Models.Business;
using GymMangamentSystem.Reposatory.Services;

namespace GymMangamentSystem.UnitTests.Services;

public class AppMapperTests
{
    private readonly AppMapper _sut = new();

    [Fact]
    public void Map_BmiDtoToEntity_MapsBusinessFields()
    {
        var result = _sut.Map<BMIRecord>(new BMIRecordDto
        {
            BMIRecordId = 7,
            Category = (int)BMICategory.Overweight,
            UserId = "user-1",
            WeightInKg = 80,
            HeightInMeters = 1.8m
        });

        Assert.Equal(7, result.BMIRecordId);
        Assert.Equal(BMICategory.Overweight, result.Category);
        Assert.Equal("user-1", result.UserId);
        Assert.Equal(80, result.WeightInKg);
        Assert.Equal(1.8m, result.HeightInMeters);
    }

    [Fact]
    public void Map_EntityToDto_MapsExpectedFields()
    {
        var result = _sut.Map<ExerciseDto>(new Exercise
        {
            ExerciseId = 3,
            ExerciseName = "Squat",
            Description = "Leg exercise",
            Repetitions = 10,
            Sets = 4,
            ExerciseCategoryId = 2
        });

        Assert.Equal(3, result.ExerciseId);
        Assert.Equal("Squat", result.ExerciseName);
        Assert.Equal(10, result.Repetitions);
        Assert.Equal(4, result.Sets);
        Assert.Equal(2, result.ExerciseCategoryId);
    }

    [Fact]
    public void Map_EntityCollection_ReturnsMappedList()
    {
        var source = new List<Notification>
        {
            new() { NotificationId = 1, Message = "First", Date = DateTime.UtcNow },
            new() { NotificationId = 2, Message = "Second", Date = DateTime.UtcNow }
        };

        var result = _sut.Map<IEnumerable<NotificationDto>>(source).ToList();

        Assert.Equal(2, result.Count);
        Assert.Equal("First", result[0].Message);
        Assert.Equal("Second", result[1].Message);
    }

    [Fact]
    public void Map_AllConfiguredPairs_AreSupported()
    {
        Assert.IsType<Attendance>(_sut.Map<Attendance>(new AttendanceDto()));
        Assert.IsType<AttendanceDto>(_sut.Map<AttendanceDto>(new Attendance()));
        Assert.IsType<Class>(_sut.Map<Class>(new ClassDto()));
        Assert.IsType<ClassDto>(_sut.Map<ClassDto>(new Class()));
        Assert.IsType<ExerciseCategory>(_sut.Map<ExerciseCategory>(new ExerciseCategoryDto()));
        Assert.IsType<ExerciseCategoryDto>(_sut.Map<ExerciseCategoryDto>(new ExerciseCategory()));
        Assert.IsType<Feedback>(_sut.Map<Feedback>(new FeedbackDto()));
        Assert.IsType<FeedbackDto>(_sut.Map<FeedbackDto>(new Feedback()));
        Assert.IsType<Meal>(_sut.Map<Meal>(new MealDto()));
        Assert.IsType<MealDto>(_sut.Map<MealDto>(new Meal()));
        Assert.IsType<MealsCategory>(_sut.Map<MealsCategory>(new MealsCategoryDto()));
        Assert.IsType<MealsCategoryDto>(_sut.Map<MealsCategoryDto>(new MealsCategory()));
        Assert.IsType<Membership>(_sut.Map<Membership>(new MembershipDto()));
        Assert.IsType<MembershipDto>(_sut.Map<MembershipDto>(new Membership()));
        Assert.IsType<NutritionPlan>(_sut.Map<NutritionPlan>(new NutritionPlanDto()));
        Assert.IsType<NutritionPlanDto>(_sut.Map<NutritionPlanDto>(new NutritionPlan()));
        Assert.IsType<WorkoutPlan>(_sut.Map<WorkoutPlan>(new WorkoutPlanDto()));
        Assert.IsType<WorkoutPlanDto>(_sut.Map<WorkoutPlanDto>(new WorkoutPlan()));
    }

    [Fact]
    public void Map_WhenPairIsUnknown_ThrowsClearException()
    {
        var exception = Assert.Throws<InvalidOperationException>(() => _sut.Map<MealDto>(new Attendance()));
        Assert.Contains("No mapping is configured", exception.Message);
    }
}
