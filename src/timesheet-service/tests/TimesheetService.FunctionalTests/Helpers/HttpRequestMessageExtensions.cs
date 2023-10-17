using System.Text;
using System.Text.Json;
using TimesheetService.Shared.Infrastructure;

namespace TimesheetService.FunctionalTests.Helpers;

public static class HttpRequestMessageExtensions
{
    public static HttpRequestMessage Create(HttpMethod method,
        Uri baseUri,
        string? path = null,
        HttpContent? content = null,
        Dictionary<string, string>? headers = null,
        Dictionary<string, List<object>?>? queryParams = null)
    {
        string fullPath;

        if (path.IsNullOrWhiteSpace())
            fullPath = baseUri.AbsoluteUri;
        else
            fullPath = baseUri.AbsoluteUri + path;

        var newUri = new Uri(fullPath);

        var request = new HttpRequestMessage(method, newUri);

        if (headers is not null && headers.Any())
            foreach (var item in headers)
                request.Headers.TryAddWithoutValidation(item.Key, item.Value);

        if (content is not null)
            request.Content = content;

        return request;
    }

    public static HttpContent CreateJsonAsContent(object value)
        => new StringContent(JsonSerializer.Serialize(value), Encoding.UTF8, "application/json");
}