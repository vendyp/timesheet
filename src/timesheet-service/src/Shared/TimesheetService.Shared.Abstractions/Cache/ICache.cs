namespace TimesheetService.Shared.Abstractions.Cache;

public interface ICache
{
    Task<T?> GetAsync<T>(string key);
    Task<IReadOnlyList<T>> GetManyAsync<T>(params string[] keys);
    Task SetAsync<T>(string key, T value, TimeSpan? expiry = null);
    Task DeleteAsync(string key);
}