using app.Domain.Exceptions;
using FluentValidation.Results;

namespace app.WebApi.Dtos.Requests
{
    public class BaseModelRequest
    {
        public BaseModelRequest() { }
        protected ValidationResult ValidationResult { get; set; }

        protected virtual bool IsValid()
        {
            throw new NotImplementedException();
        }

        public void ThrowIfNotValid()
        {
            if (!IsValid())
            {
                var errors = ValidationResult.Errors
                    .Select(e => e.ErrorMessage)
                    .ToList();

                throw new DomainException(errors);
            }
        }
    }
}