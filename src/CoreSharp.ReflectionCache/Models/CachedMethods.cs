using CoreSharp.ReflectionCache.Models.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CoreSharp.ReflectionCache.Models;

public sealed class CachedMethods : CachedCollectionBase<CachedMethod>
{
    // Constructors 
    internal CachedMethods(Type type)
        : base(GetCachedMethods(type))
    {
    }

    // Methods 
    private static IEnumerable<CachedMethod> GetCachedMethods(Type type)
        => type
            ?.GetMethods()
            .Select(method => new CachedMethod(method));
}