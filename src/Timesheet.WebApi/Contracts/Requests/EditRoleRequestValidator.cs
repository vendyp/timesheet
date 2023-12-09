using Timesheet.WebApi.Validators;
using FluentValidation;

namespace Timesheet.WebApi.Contracts.Requests;

public class EditRoleRequestValidator : AbstractValidator<EditRoleRequest>
{
    public EditRoleRequestValidator()
    {
        RuleFor(e => e.Payload).NotNull();
        When(e => e.Payload != null!, () =>
        {
            RuleFor(e => e.Payload.Description)
                .MaximumLength(512)
                .SetValidator(new AsciiOnlyValidator());
            RuleFor(e => e.Payload.Scopes).NotNull().Must(e => e.Count == e.Distinct().Count());
            RuleForEach(e => e.Payload.Scopes)
                .MaximumLength(100)
                .SetValidator(new AsciiOnlyValidator());
        });
    }
}