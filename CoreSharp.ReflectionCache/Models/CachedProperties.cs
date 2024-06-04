using CoreSharp.ReflectionCache.Models.Abstracts;
using System.Reflection;

namespace CoreSharp.ReflectionCache.Models;

public sealed class CachedProperties(Type? type)
    : CachedDictionaryBase<CachedProperty>(GetCachedPropertiesDictionary(type))
{

    // Methods 
    private static IReadOnlyDictionary<string, CachedProperty>? GetCachedPropertiesDictionary(Type? type)
        => type?.GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .ToDictionary(
                property => property.Name,
                property => new CachedProperty(property));
}
