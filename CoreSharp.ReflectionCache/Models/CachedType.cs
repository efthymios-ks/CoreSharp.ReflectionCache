using CoreSharp.ReflectionCache.Services;
using Microsoft.Extensions.Caching.Memory;
using System.Diagnostics;

namespace CoreSharp.ReflectionCache.Models;

[DebuggerDisplay("{DebuggerDisplay,nq}")]
public sealed class CachedType
{
    // Fields 
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private CachedConstructors? _constructors;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private CachedAttributes? _attributes;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private CachedProperties? _properties;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private CachedFields? _fields;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private CachedMethods? _methods;

    // Constructors
    private CachedType(Type type)
    {
        ArgumentNullException.ThrowIfNull(type);

        BaseType = type;
    }

    // Properties 
    private static ICacheStorage Cache { get; }
        = new CacheStorage(new MemoryCache(new MemoryCacheOptions()));

    private string DebuggerDisplay
        => FullName;

    public Type BaseType { get; }

    public string FullName
        => BaseType.FullName!;

    public string Name
        => BaseType.Name;

    public CachedConstructors Constructors
        => _constructors ??= new CachedConstructors(BaseType);

    public CachedAttributes Attributes
        => _attributes ??= new CachedAttributes(BaseType);

    public CachedProperties Properties
        => _properties ??= new CachedProperties(BaseType);

    public CachedFields Fields
        => _fields ??= new CachedFields(BaseType);

    public CachedMethods Methods
        => _methods ??= new CachedMethods(BaseType);

    // Methods 
    public static CachedType Get<TType>()
        => Get(typeof(TType));

    public static CachedType Get(Type? type)
        => Get(type, TimeSpan.FromMinutes(15));

    public static CachedType Get<TType>(TimeSpan cacheDuration)
        => Get(typeof(TType), cacheDuration);

    public static CachedType Get(Type? type, TimeSpan cacheDuration)
    {
        ArgumentNullException.ThrowIfNull(type);

        return Cache.GetOrAdd(type.FullName!, () => new CachedType(type), cacheDuration);
    }
}
