using app.Domain.Exceptions;
using FluentValidation.Results;

namespace app.Domain.Models.Base
{
    public abstract class BaseModel
    {
        protected BaseModel() { }

        public long Id { get; set; }
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