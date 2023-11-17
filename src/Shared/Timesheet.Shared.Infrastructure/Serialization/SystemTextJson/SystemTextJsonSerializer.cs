using System.Text.Json;
using System.Text.Json.Serialization;
using Timesheet.Shared.Abstractions.Serialization;

namespace Timesheet.Shared.Infrastructure.Serialization.SystemTextJson;

internal sealed class SystemTextJsonSerializer : IJsonSerializer
{
    private static readonly JsonSerializerOptions Options = new()
    {
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        Converters = { new JsonStringEnumConverter(), new DateOnlyConverter(), new TimeOnlyConverter() }
    };

    public string Serialize<T>(T value) => JsonSerializer.Serialize(value, Options);

    public T? Deserialize<T>(string value) => JsonSerializer.Deserialize<T>(value, Options);

    public object? Deserialize(string value, Type type) => JsonSerializer.Deserialize(value, type, Options);
}