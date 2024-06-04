using Microsoft.Extensions.Caching.Memory;

namespace CoreSharp.ReflectionCache.Services;

public sealed class CacheStorage(IMemoryCache memoryCache) : ICacheStorage
{
    private readonly IMemoryCache _memoryCache = memoryCache;

    public TValue GetOrAdd<TValue>(string key, Func<TValue> valueFactory, TimeSpan duration)
    {
        if (_memoryCache.TryGetValue<TValue>(key, out var value))
        {
            return value!;
        }

        value = valueFactory();
        value = _memoryCache.Set(key, value, duration);
        return value!;
    }
}
