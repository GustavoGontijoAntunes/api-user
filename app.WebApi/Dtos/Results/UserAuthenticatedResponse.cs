namespace app.WebApi.Dtos.Results
{
    public class UserAuthenticatedResponse
    {
        public bool Authenticated { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? Expiration { get; set; }
        public string? Message { get; set; }
        public string? AccessToken { get; set; }
    }
}