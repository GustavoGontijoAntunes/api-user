namespace app.Domain.Models.Authentication
{
    public class SecurityException : Exception
    {
        public SecurityException(string securityMessage) : base(securityMessage)
        {

        }
    }
}