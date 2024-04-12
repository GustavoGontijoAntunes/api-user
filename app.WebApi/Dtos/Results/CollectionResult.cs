namespace app.WebApi.Dtos.Results
{
    public class CollectionResult<T> where T : class
    {
        public int PageIndex { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public bool HasPrevious { get; set; }
        public bool HasNext { get; set; }
        public List<T>? Items { get; set; }
    }
}