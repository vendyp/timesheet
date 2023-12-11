using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Vendyp.Timesheet.Shared.Abstractions.Models;

namespace Vendyp.Timesheet.Shared.Infrastructure.Api;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "Exception occurred: {Message}", exception.Message);

        var env = httpContext.RequestServices.GetRequiredService<IWebHostEnvironment>();

        var error = env.IsProduction()
            ? Error.Create("Internal server error")
            : Error.Create("Internal server error, please check inner message", exception.Message, 500);

        // this code is added to override argument null exception from Fluent Validator of its request
        // why?, because at this current state, I still do not know how to override this more clear and cleanly
        // then I decided to override the exception when the request object is passed null to validator
        // it will throw error exception type of ArgumentNullException with message contains
        // instanceToValidate, then override its message and inner message and return it.
        if (exception is ArgumentNullException argEx && !env.IsProduction())
        {
            if (argEx.Message.Contains("instanceToValidate"))
            {
                httpContext.Response.StatusCode = 400;
                await httpContext.Response
                    .WriteAsJsonAsync(Error.Create("Invalid parameter",
                            "To developer, please check your body request/query/path, this error occur when failed to deserializing your request",
                            400),
                        cancellationToken);
                return true;
            }
        }

        httpContext.Response.StatusCode = 500;
        await httpContext.Response
            .WriteAsJsonAsync(error, cancellationToken);
        return true;
    }
}