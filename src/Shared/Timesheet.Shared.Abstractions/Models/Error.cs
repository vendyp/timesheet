namespace Timesheet.Shared.Abstractions.Models;

public sealed class Error
{
    public Error(string? message, string? innerMessage)
    {
        Message = message;
        InnerMessage = innerMessage;
        Code = 400;
    }

    public Error(string? message, string? innerMessage, List<ValidationError>? value)
    {
        Message = message;
        InnerMessage = innerMessage;
        Code = 400;

        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        if (!(!string.IsNullOrWhiteSpace(environment) && environment.ToUpper() == "PRODUCTION"))
            Payload = value;
    }

    public Error(string? message, string? innerMessage, int code) : this(message, innerMessage)
    {
        Code = code;
    }

    public string? InnerMessage { get; }

    public string? Message { get; }

    /// <summary>
    /// Default value is 0.
    /// </summary>
    public int Code { get; }

    public List<ValidationError>? Payload { get; }

    public static Error Create(string? message) => new(message, null);
    public static Error Create(string? message, int code) => new(message, null, code);
    public static Error Create(string? message, string innerMessage, int code) => new(message, innerMessage, code);
    public static Error Create(string? message, List<ValidationError>? value) => new(message, null, value);
    public static Error? Ignored() => null;
}