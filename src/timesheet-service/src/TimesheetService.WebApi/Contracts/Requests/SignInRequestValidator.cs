using TimesheetService.WebApi.Validators;
using FluentValidation;

namespace TimesheetService.WebApi.Contracts.Requests;

public class SignInRequestValidator : AbstractValidator<SignInRequest>
{
    public SignInRequestValidator()
    {
        RuleFor(e => e.Username).NotNull().NotEmpty().MaximumLength(100).MinimumLength(3)
            .SetValidator(new AsciiOnlyValidator());
        RuleFor(e => e.Password).NotNull().NotEmpty().MaximumLength(256);
    }
}