namespace app.WebApi.Dtos.Requests
{
    public class UserGet : Filter
    {
        public string? Name { get; set; }
        public string? Login { get; set; }
        public long? ProfileId { get; set; }
    }
}