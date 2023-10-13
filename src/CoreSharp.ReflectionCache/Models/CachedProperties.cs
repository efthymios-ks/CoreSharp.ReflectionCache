using CoreSharp.ReflectionCache.Models.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CoreSharp.ReflectionCache.Models;

public sealed class CachedProperties : CacheDictionaryBase<CachedProperty>
{
    // Constructors 
    public CachedProperties(Type type)
        : base(GetCachedPropertiesDictionary(type))
    {
    }

    // Methods 
    private static IReadOnlyDictionary<string, CachedProperty> GetCachedPropertiesDictionary(Type type)
        => type?.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .ToDictionary(
                    property => property.Name,
                    property => new CachedProperty(property));
}
