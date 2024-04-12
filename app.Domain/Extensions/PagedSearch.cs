using app.Domain.Models.Base;

namespace app.Domain.Extensions
{
    public class PagedSearch : BaseModelSearch
    {
        const int minPageIndex = 0;
        private int _pageIndex = 0;

        const int maxPageSize = int.MaxValue;
        private int _pageSize = 10;

        public int PageIndex
        {
            get => _pageIndex;
            set => _pageIndex = (value < minPageIndex) ? minPageIndex : value;
        }

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > maxPageSize) ? maxPageSize : value;
        }
        public string? Sort { get; set; }
    }
}