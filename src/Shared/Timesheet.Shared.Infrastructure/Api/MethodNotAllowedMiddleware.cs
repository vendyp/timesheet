using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http;
using Timesheet.Shared.Abstractions.Models;

namespace Timesheet.Shared.Infrastructure.Api;

public class MethodNotAllowedMiddleware
{
    private readonly RequestDelegate _next;

    public MethodNotAllowedMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        await _next(context);

        if (context.Response.StatusCode == StatusCodes.Status405MethodNotAllowed)
        {
            context.Response.ContentType = "application/json";
            var serializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };
            var error = JsonSerializer.Serialize(
                Error.Create(
                    $"Http method {context.Request.Method} is not allowed at this path {context.Request.Path}",
                    StatusCodes.Status405MethodNotAllowed),
                serializerOptions);
            await context.Response.WriteAsync(error);
        }
    }
}