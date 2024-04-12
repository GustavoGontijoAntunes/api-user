using app.Domain.Models.Base;
using app.Domain.Resources;
using FluentValidation;

namespace app.Domain.Models
{
    public class Permission : BaseModel
    {
        public Permission() { }

        public string Name { get; set; }
        public string Description { get; set; }
        public List<Profile> Profiles { get; set; }

        protected override bool IsValid()
        {
            ValidationResult = new PermissionValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class PermissionValidation : AbstractValidator<Permission>
    {
        public PermissionValidation()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage(CustomMessages.NameIsRequiredToCreatePermission)
                .MaximumLength(50).WithMessage(CustomMessages.PermissionNameMaxLength);

            RuleFor(x => x.Description).NotEmpty().WithMessage(CustomMessages.DescriptionIsRequiredToCreatePermission)
                .MaximumLength(200).WithMessage(CustomMessages.PermissionDescriptionMaxLength);
        }
    }
}