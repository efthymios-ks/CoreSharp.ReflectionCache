using CoreSharp.ReflectionCache.Models.Abstracts;

namespace CoreSharp.ReflectionCache.Models;

public sealed class CachedConstructors : CachedCollectionBase<CachedConstructor>
{
    // Constructors 
    internal CachedConstructors(Type? type)
        : base(GetCachedConstructors(type))
    {
    }

    // Methods 
    private static IEnumerable<CachedConstructor>? GetCachedConstructors(Type? type)
        => type
            ?.GetConstructors()
            .Select(constructor => new CachedConstructor(constructor));
}
