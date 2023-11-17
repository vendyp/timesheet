using FluentValidation;

namespace Timesheet.WebApi.Contracts.Requests;

public class RefreshTokenRequestValidator : AbstractValidator<RefreshTokenRequest>
{
    public RefreshTokenRequestValidator()
    {
        RuleFor(e => e.RefreshToken).NotNull().NotEmpty();
    }
}