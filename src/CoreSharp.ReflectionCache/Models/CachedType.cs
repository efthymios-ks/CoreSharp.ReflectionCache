using Microsoft.Extensions.Caching.Memory;
using System;
using System.Diagnostics;

namespace CoreSharp.ReflectionCache.Models;

[DebuggerDisplay("{DebuggerDisplay,nq}")]
public sealed class CachedType
{
    // Fields
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private static IMemoryCache _cachedTypes = new MemoryCache(new MemoryCacheOptions());

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private CachedAttributes _attributes;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private CachedProperties _properties;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private CachedFields _fields;

    // Constructors
    private CachedType(Type type)
    {
        ArgumentNullException.ThrowIfNull(type);
        BaseType = type;
    }

    // Properties 
    private static IMemoryCache CachedTypes
        => _cachedTypes ??= new MemoryCache(new MemoryCacheOptions());

    private string DebuggerDisplay
        => FullName;

    public Type BaseType { get; }

    public string FullName
        => BaseType.FullName;

    public string Name
        => BaseType.Name;

    public CachedAttributes Attributes
        => _attributes ??= new CachedAttributes(BaseType);

    public CachedProperties Properties
        => _properties ??= new CachedProperties(BaseType);

    public CachedFields Fields
        => _fields ??= new CachedFields(BaseType);

    // Methods 
    public static CachedType Get<TType>()
        => Get(typeof(TType));

    public static CachedType Get(Type type)
        => Get(type, TimeSpan.FromMinutes(15));

    public static CachedType Get<TType>(TimeSpan cacheDuration)
        => Get(typeof(TType), cacheDuration);

    public static CachedType Get(Type type, TimeSpan cacheDuration)
    {
        if (CachedTypes.TryGetValue<CachedType>(type, out var cachedType))
        {
            return cachedType;
        }

        cachedType = new CachedType(type);
        CachedTypes.Set(type, cachedType, cacheDuration);
        return cachedType;
    }
}
