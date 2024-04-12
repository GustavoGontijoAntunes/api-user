namespace app.WebApi.Dtos.Requests
{
    public class UserPost
    {
        public string? Name { get; set; }
        public string? Login { get; set; }
        public string? Password { get; set; }
        public long? ProfileId { get; set; }
    }
}