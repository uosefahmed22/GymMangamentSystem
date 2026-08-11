namespace GymMangamentSystem.Core.Models.Common
{
    public sealed class PaginationParameters
    {
        private const int MaxPageSize = 100;
        private int _pageSize = 20;

        public int PageNumber { get; set; } = 1;

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = Math.Clamp(value, 1, MaxPageSize);
        }

        public int Skip => (Math.Max(PageNumber, 1) - 1) * PageSize;
    }
}
