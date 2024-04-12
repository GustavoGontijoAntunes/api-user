namespace app.Domain.Extensions
{
    public class PagedList<T>
    {
        public int PageIndex { get; private set; }
        public int TotalPages { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }
        public List<T> Items { get; set; }

        public bool HasPrevious => PageIndex > 0;
        public bool HasNext => (PageIndex + 1) < TotalPages;

        public PagedList(List<T> items, int count, int pageNumber, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            PageIndex = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            Items = items;
        }

        public static PagedList<T> ToPagedList(IQueryable<T> source, int pageIndex, int pageSize)
        {
            var count = source.Count();
            var items = source.Skip((pageIndex) * pageSize).Take(pageSize).ToList();

            return new PagedList<T>(items, count, pageIndex, pageSize);
        }
    }
}