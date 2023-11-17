using Timesheet.WebApi.Validators;
using FluentValidation;

namespace Timesheet.WebApi.Contracts.Requests;

public class CreateUserRequestValidator : AbstractValidator<CreateUserRequest>
{
    public CreateUserRequestValidator()
    {
        RuleFor(e => e.Username).NotNull().NotEmpty().MaximumLength(256).SetValidator(new AsciiOnlyValidator());
        RuleFor(e => e.Password).NotNull().NotEmpty().MaximumLength(256);
        RuleFor(e => e.Fullname).NotNull().NotEmpty().MaximumLength(256).SetValidator(new AsciiOnlyValidator());
        RuleFor(e => e.RoleId).NotNull();
        RuleFor(e => e.EmailAddress).NotEmpty().MaximumLength(256).EmailAddress();
    }
}