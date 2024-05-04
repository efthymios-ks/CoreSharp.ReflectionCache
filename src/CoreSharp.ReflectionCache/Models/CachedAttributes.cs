using CoreSharp.ReflectionCache.Models.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CoreSharp.ReflectionCache.Models;

public sealed class CachedAttributes : CachedCollectionBase<Attribute>
{
    // Constructors 
    internal CachedAttributes(MemberInfo memberInfo)
        : this(memberInfo?.GetCustomAttributes<Attribute>(true))
    {
    }

    internal CachedAttributes(IEnumerable<Attribute> attributes)
        : base(attributes)
    {
    }

    // Methods 
    public TAttribute OfType<TAttribute>()
        where TAttribute : Attribute
        => OfType(typeof(TAttribute)) as TAttribute;

    public Attribute OfType(Type attributeType)
    {
        ArgumentNullException.ThrowIfNull(attributeType);
        return Array.Find(Source, attribute => attribute.GetType() == attributeType);
    }

    public TAttribute[] OfTypeAll<TAttribute>()
        where TAttribute : Attribute
        => Source.OfType<TAttribute>().ToArray();

    public Attribute[] OfTypeAll(Type attributeType)
    {
        ArgumentNullException.ThrowIfNull(attributeType);
        return Array.FindAll(Source, attribute => attribute.GetType() == attributeType);
    }
}
