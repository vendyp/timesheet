namespace Timesheet.Shared.Abstractions.Helpers;

public static class StringExtensions
{
    public static string ToCamelCase(this string s) => $"{char.ToLowerInvariant(s[0])}{s[1..]}";
}