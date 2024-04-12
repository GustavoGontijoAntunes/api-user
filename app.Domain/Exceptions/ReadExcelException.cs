using System.Runtime.Serialization;

namespace app.Domain.Exceptions
{
    [Serializable()]
    public class ReadExcelException : Exception
    {
        public List<(string, int)>? ReadErrors { get; private set; }

        public ReadExcelException() { }

        public ReadExcelException(string message) : base(message) { }

        public ReadExcelException(string message, Exception innerException) :
           base(message, innerException)
        { }

        public ReadExcelException(List<(string, int)> readErrors)
        {
            ReadErrors = readErrors;
        }

        protected ReadExcelException(SerializationInfo info,
           StreamingContext context) : base(info, context) { }
    }
}