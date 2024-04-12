namespace app.WebApi.Dtos.Requests
{
    public class UserPut
    {
        public long? Id { get; set; }
        public string? Name { get; set; }
        public string? Login { get; set; }
        public long? ProfileId { get; set; }
    }
}