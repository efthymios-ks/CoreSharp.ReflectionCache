namespace CoreSharp.ReflectionCache.Services;

public interface ICacheStorage
{
    TValue GetOrAdd<TValue>(string key, Func<TValue> valueFactory, TimeSpan duration);
}
