using app.Domain.Resources;
using FluentValidation;

namespace app.ExcelAdapter.Models
{
    public class PermissionExcel
    {
        public string name { get; set; }
        public string description { get; set; }
        public long line_number { get; set; }
    }

    public class PermissionExcelValidation : AbstractValidator<PermissionExcel>
    {
        public PermissionExcelValidation()
        {
            RuleFor(x => x.name).NotEmpty().WithMessage(y => String.Format(CustomMessages.NameIsRequiredToCreatePermissionExcel, y.line_number))
                .MaximumLength(50).WithMessage(y => String.Format(CustomMessages.PermissionNameMaxLengthExcel, y.line_number));
            RuleFor(x => x.description).MaximumLength(200).WithMessage(y => String.Format(CustomMessages.PermissionDescriptionMaxLengthExcel, y.line_number));
        }
    }
}