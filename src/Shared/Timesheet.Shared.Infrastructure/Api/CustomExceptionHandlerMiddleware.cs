using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using Timesheet.Shared.Abstractions.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Timesheet.Shared.Infrastructure.Api
{
    internal class CustomExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<CustomExceptionHandlerMiddleware> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomExceptionHandlerMiddleware"/> class.
        /// </summary>
        /// <param name="next">The delegate pointing to the next middleware in the chain.</param>
        /// <param name="logger">The logger.</param>
        public CustomExceptionHandlerMiddleware(RequestDelegate next, ILogger<CustomExceptionHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        /// <summary>
        /// Invokes the exception handler middleware with the specified <see cref="HttpContext"/>.
        /// </summary>
        /// <param name="httpContext">The HTTP httpContext.</param>
        /// <returns>The task that can be awaited by the next middleware.</returns>
        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);

                if (httpContext.Response.StatusCode == (int)HttpStatusCode.Unauthorized)
                    await HandleExceptionAsync(httpContext, null, "You are not authorized");
                if (httpContext.Response.StatusCode == (int)HttpStatusCode.Forbidden)
                    await HandleExceptionAsync(httpContext, null, "You are forbid to access this resource.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred: {Message}", ex.Message);

                await HandleExceptionAsync(httpContext, ex);
            }
        }

        /// <summary>
        /// Handles the specified <see cref="Exception"/> for the specified <see cref="HttpContext"/>.
        /// </summary>
        /// <param name="httpContext">The HTTP httpContext.</param>
        /// <param name="exception">The exception.</param>
        /// <param name="message"></param>
        /// <returns>The HTTP response that is modified based on the exception.</returns>
        private static async Task HandleExceptionAsync(HttpContext httpContext, Exception? exception = null,
            string? message = null)
        {
            var (httpStatusCode, errors) = GetHttpStatusCodeAndErrors(exception, httpContext.Response, message);

            httpContext.Response.ContentType = "application/json";

            httpContext.Response.StatusCode = httpStatusCode;

            var serializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };

            var response = JsonSerializer.Serialize(new ApiErrorResponse(errors), serializerOptions);

            await httpContext.Response.WriteAsync(response);
        }

        /// <summary>
        /// Extracts the HTTP status code and a collection of errors based on the specified exception.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <param name="httpStatusCode"></param>
        /// <param name="message"></param>
        /// <returns>The HTTP status code and a collection of errors based on the specified exception.</returns>
        private static (int HttpStatusCode, IReadOnlyCollection<Error> Errors) GetHttpStatusCodeAndErrors(
            Exception? exception, HttpResponse httpStatusCode, string? message) =>
            exception switch
            {
                Exceptions.ValidationException validationException => ((int)HttpStatusCode.BadRequest,
                    validationException.Errors),
                _ => (
                    httpStatusCode.StatusCode == (int)HttpStatusCode.OK
                        ? (int)HttpStatusCode.InternalServerError
                        : httpStatusCode.StatusCode, new[]
                    {
                        Error.Create(exception == null ? message : exception.Message,
                            exception == null ? httpStatusCode.StatusCode : (int)HttpStatusCode.InternalServerError)
                    })
            };
    }
}