using FluentValidation;

namespace TimesheetService.WebApi.Contracts.Requests;

public class UpdateUserRequestPayloadValidator : AbstractValidator<UpdateUserRequestPayload>
{
    public UpdateUserRequestPayloadValidator()
    {
        RuleFor(e => e.Fullname).NotNull().NotEmpty();
    }
}