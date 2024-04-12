using app.Domain.Exceptions;
using FluentValidation.Results;

namespace app.Domain.Models.Base
{
    public class BaseModelSearch
    {
        protected ValidationResult ValidationResult { get; set; }

        public BaseModelSearch()
        {
            ValidationResult = new ValidationResult();
        }

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