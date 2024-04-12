using app.Domain.Extensions;
using app.Domain.Resources;
using FluentValidation;

namespace app.Domain.Models.Filters
{
    public class ProfileSearch : PagedSearch
    {
        public string? Name { get; set; }

        protected override bool IsValid()
        {
            ValidationResult = new ProfileSearchValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class ProfileSearchValidation : AbstractValidator<ProfileSearch>
    {
        public ProfileSearchValidation()
        {
            RuleFor(x => x.Name).MaximumLength(100).WithMessage(CustomMessages.ProfileNameMaxLength);
        }
    }
}