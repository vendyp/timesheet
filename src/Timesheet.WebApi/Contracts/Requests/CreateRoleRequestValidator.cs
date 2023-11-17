using Timesheet.WebApi.Scopes;
using FluentValidation;

namespace Timesheet.WebApi.Contracts.Requests;

public class CreateRoleRequestValidator : AbstractValidator<CreateRoleRequest>
{
    public CreateRoleRequestValidator()
    {
        RuleFor(e => e.Name).NotNull().NotEmpty().MaximumLength(256);
        When(e => e.Scopes.Any(), () =>
        {
            RuleFor(e => e.Scopes).Must(e => e.Count == e.Distinct().Count());

            var list = ScopeManager.Instance.GetAllScopes();

            RuleForEach(e => e.Scopes).Must(e => list.Contains(e));
        });
    }
}