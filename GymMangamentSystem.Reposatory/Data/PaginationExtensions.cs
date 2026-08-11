using GymMangamentSystem.Core.Models.Common;

namespace GymMangamentSystem.Reposatory.Data
{
    internal static class PaginationExtensions
    {
        public static IQueryable<T> ApplyPagination<T>(
            this IQueryable<T> query,
            PaginationParameters? pagination)
        {
            pagination ??= new PaginationParameters();
            return query.Skip(pagination.Skip).Take(pagination.PageSize);
        }
    }
}
