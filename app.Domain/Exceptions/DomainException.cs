using System.Runtime.Serialization;

namespace app.Domain.Exceptions
{
    [Serializable()]
    public class DomainException : Exception
    {
        public List<string>? ValidationErrors { get; private set; }

        public DomainException() { }

        public DomainException(string message) : base(message) { }

        public DomainException(string message, Exception innerException) :
           base(message, innerException)
        { }

        public DomainException(List<string> validationErrors)
        {
            ValidationErrors = validationErrors;
        }

        protected DomainException(SerializationInfo info,
           StreamingContext context) : base(info, context) { }
    }
}