using app.Domain.Models.Base;
using app.Domain.Resources;
using FluentValidation;
using System.Collections;

namespace app.Domain.Models.Authentication
{
    public class User : BaseModel
    {
        public User() { }

        public string Name { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public virtual string NewPassword { get; set; }
        public virtual string? Token { get; set; }
        public string? SessionId { get; set; }
        public long ProfileId { get; set; }
        public virtual IEnumerable Permissions { get; set; }
        public Profile Profile { get; set; }

        protected override bool IsValid()
        {
            ValidationResult = new UserValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class UserValidation : AbstractValidator<User>
    {
        public UserValidation()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage(CustomMessages.NameIsRequiredToCreateUser)
                .MaximumLength(100).WithMessage(CustomMessages.UserNameMaxLength);

            RuleFor(x => x.Login).NotEmpty().WithMessage(CustomMessages.LoginIsRequiredToCreateUser)
                .MaximumLength(20).WithMessage(CustomMessages.UserLoginMaxLength);

            RuleFor(x => x.Password).NotEmpty().When(y => y.Id == 0).WithMessage(CustomMessages.PasswordIsRequiredToCreateUser);

            RuleFor(x => x.ProfileId).NotEmpty().WithMessage(CustomMessages.ProfileIsRequiredToCreateUser);
        }
    }
}