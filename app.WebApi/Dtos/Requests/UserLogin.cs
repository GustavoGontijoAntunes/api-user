using app.Domain.Resources;
using FluentValidation;

namespace app.WebApi.Dtos.Requests
{
    public class UserLogin : BaseModelRequest
    {
        public string? Login { get; set; }
        public string? Password { get; set; }

        protected override bool IsValid()
        {
            ValidationResult = new UserLoginValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class UserLoginValidation : AbstractValidator<UserLogin>
    {
        public UserLoginValidation()
        {
            RuleFor(x => x.Login).NotEmpty().WithMessage(CustomMessages.LoginIsRequiredToLogin);
            RuleFor(x => x.Password).NotEmpty().WithMessage(CustomMessages.PasswordIsRequiredToLogin);
        }
    }
}