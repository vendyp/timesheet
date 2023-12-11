using Vendyp.Timesheet.Shared.Abstractions.Helpers;

namespace Vendyp.Timesheet.Shared.Abstractions.Models;

public class ValidationError
{
    public ValidationError(string propertyName, object attemptedValue, string errorCode, string message)
    {
        PropertyName = propertyName.ToCamelCase();
        AttemptedValue = attemptedValue;
        ErrorCode = errorCode;
        Message = message;
    }

    public string PropertyName { get; set; }
    public object? AttemptedValue { get; set; }
    public string ErrorCode { get; set; }
    public string Message { get; set; }
}