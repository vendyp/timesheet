namespace TimesheetService.Shared.Abstractions.Serialization;

public interface IJsonSerializer
{
    string Serialize<T>(T value);
    T? Deserialize<T>(string value);
    object? Deserialize(string value, Type type);
}