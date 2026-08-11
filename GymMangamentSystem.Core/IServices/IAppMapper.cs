namespace GymMangamentSystem.Core.IServices;

public interface IAppMapper
{
    TDestination Map<TDestination>(object source);
}
