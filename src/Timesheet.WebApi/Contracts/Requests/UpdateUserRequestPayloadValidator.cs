using FluentValidation;

namespace Timesheet.WebApi.Contracts.Requests;

public class UpdateUserRequestPayloadValidator : AbstractValidator<UpdateUserRequestPayload>
{
    public UpdateUserRequestPayloadValidator()
    {
        RuleFor(e => e.Fullname).NotNull().NotEmpty();
    }
}