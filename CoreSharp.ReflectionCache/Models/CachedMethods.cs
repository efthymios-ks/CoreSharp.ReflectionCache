using CoreSharp.ReflectionCache.Models.Abstracts;

namespace CoreSharp.ReflectionCache.Models;

public sealed class CachedMethods : CachedCollectionBase<CachedMethod>
{
    // Constructors 
    internal CachedMethods(Type? type)
        : base(GetCachedMethods(type))
    {
    }

    // Methods 
    private static IEnumerable<CachedMethod>? GetCachedMethods(Type? type)
        => type?.GetMethods()
            .Select(method => new CachedMethod(method));
}
