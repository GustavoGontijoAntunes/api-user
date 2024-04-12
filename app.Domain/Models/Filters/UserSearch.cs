using app.Domain.Extensions;
using app.Domain.Resources;
using FluentValidation;

namespace app.Domain.Models.Filters
{
    public class UserSearch : PagedSearch
    {
        public string? Name { get; set; }
        public string? Login { get; set; }
        public long? ProfileId { get; set; }

        protected override bool IsValid()
        {
            ValidationResult = new UserSearchValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class UserSearchValidation : AbstractValidator<UserSearch>
    {
        public UserSearchValidation()
        {
            RuleFor(x => x.Name).MaximumLength(100).WithMessage(CustomMessages.UserNameMaxLength);
            RuleFor(x => x.Login).MaximumLength(20).WithMessage(CustomMessages.UserLoginMaxLength);
        }
    }
}