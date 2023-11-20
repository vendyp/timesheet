using FluentValidation;
using Timesheet.WebApi.Validators;

namespace Timesheet.WebApi.Contracts.Requests;

public class CreateRoleRequestValidator : AbstractValidator<CreateRoleRequest>
{
    public CreateRoleRequestValidator()
    {
        RuleFor(e => e.Name).NotNull()
            .NotEmpty()
            .MaximumLength(256)
            .SetValidator(new AsciiOnlyValidator());

        RuleFor(e => e.Description).MaximumLength(512)
            .SetValidator(new AsciiOnlyValidator());

        RuleFor(e => e.Scopes).NotNull().NotEmpty();

        RuleForEach(e => e.Scopes)
            .MaximumLength(100)
            .Must(e => !string.IsNullOrWhiteSpace(e));
    }
}