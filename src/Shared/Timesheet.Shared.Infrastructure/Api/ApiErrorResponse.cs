using Timesheet.Shared.Abstractions.Models;

namespace Timesheet.Shared.Infrastructure.Api
{
    /// <summary>
    /// Represents API an error response.
    /// </summary>
    public class ApiErrorResponse
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApiErrorResponse"/> class.
        /// </summary>
        /// <param name="innerErrors">The errors.</param>
        public ApiErrorResponse(IReadOnlyCollection<Error> innerErrors)
        {
            Payload = null;

            InnerErrors = innerErrors;
            var error = InnerErrors.First();
            if (InnerErrors.Count == 1)
                InnerErrors = null;

            InnerMessage = error.InnerMessage;
            Message = error.Message;
            Code = error.Code;
            Payload = error.Payload;

            if (Code == 0) Code = 400;

            if (InnerErrors == null || !InnerErrors.Any() ||
                Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")?.ToUpper() != "PRODUCTION") return;
            InnerErrors = null;
            InnerMessage = null;
        }

        /// <summary>
        /// Gets the errors.
        /// </summary>
        public IReadOnlyCollection<Error>? InnerErrors { get; }

        public string? InnerMessage { get; }

        public string? Message { get; }

        /// <summary>
        /// Default value is 0.
        /// </summary>
        public int Code { get; }

        public object? Payload { get; }
    }
}