using FluentValidation.Results;

namespace TimesheetService.WebApi.Client.Validators;

public static class ValidationResultWrapper
{
    public static List<ValidationError>? Construct(this ValidationResult result)
    {
        return !result.Errors.Any()
            ? null
            : result.Errors.Select(item =>
                    new ValidationError(item.PropertyName, item.AttemptedValue, item.ErrorCode, item.ErrorMessage))
                .ToList();
    }
}