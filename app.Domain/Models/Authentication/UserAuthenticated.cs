namespace app.Domain.Models.Authentication
{
    public class UserAuthenticated
    {
        public bool Authenticated { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? Expiration { get; set; }
        public string? Message { get; set; }
        public string? Acesstoken { get; set; }
        public string? SessionId { get; set; }
    }
}