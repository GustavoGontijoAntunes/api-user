namespace app.WebApi.Dtos.Requests
{
    public class Filter
    {
        public int? PageSize { get; set; } = 20;
        public int? PageIndex { get; set; } = 0;
        public string? Sort { get; set; }
    }
}