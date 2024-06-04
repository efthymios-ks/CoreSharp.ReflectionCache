using CoreSharp.ReflectionCache.Models.Abstracts;

namespace CoreSharp.ReflectionCache.Models;

public sealed class CachedFields : CachedDictionaryBase<CachedField>
{
    // Constructors 
    internal CachedFields(Type? type)
        : base(GetCachedFieldsDictionary(type))
    {
    }

    // Methods 
    private static IReadOnlyDictionary<string, CachedField>? GetCachedFieldsDictionary(Type? type)
        => type?.GetFields()
            .ToDictionary(
                field => field.Name,
                field => new CachedField(field));
}
