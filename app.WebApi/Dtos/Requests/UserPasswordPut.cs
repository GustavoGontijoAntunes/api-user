using app.Domain.Resources;
using FluentValidation;

namespace app.WebApi.Dtos.Requests
{
    public class UserPasswordPut : BaseModelRequest
    {
        public long? Id { get; set; }
        public string? OldPassword { get; set; }
        public string? NewPassword { get; set; }

        protected override bool IsValid()
        {
            ValidationResult = new UserPasswordPutValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class UserPasswordPutValidation : AbstractValidator<UserPasswordPut>
    {
        public UserPasswordPutValidation()
        {
            RuleFor(x => x.OldPassword).NotEmpty().WithMessage(CustomMessages.OldPasswordIsRequiredToChangeUserPassword);
            RuleFor(x => x.NewPassword).NotEmpty().WithMessage(CustomMessages.NewPasswordIsRequiredToChangeUserPassword);
        }
    }
}