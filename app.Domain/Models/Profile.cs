using app.Domain.Models.Authentication;
using app.Domain.Models.Base;
using app.Domain.Resources;
using FluentValidation;

namespace app.Domain.Models
{
    public class Profile : BaseModel
    {
        public Profile() { }

        public string Name { get; set; }
        public List<Permission> Permissions { get; set; }
        public virtual List<long> PermissionsIds { get; set; }
        public List<User> Users { get; set; }

        protected override bool IsValid()
        {
            ValidationResult = new ProfileValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class ProfileValidation : AbstractValidator<Profile>
    {
        public ProfileValidation()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage(CustomMessages.NameIsRequiredToCreateProfile)
                .MaximumLength(50).WithMessage(CustomMessages.ProfileNameMaxLength);
        }
    }
}