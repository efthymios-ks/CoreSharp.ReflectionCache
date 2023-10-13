using System;
using System.Diagnostics;
using System.Reflection;

namespace CoreSharp.ReflectionCache.Models.Abstracts;

[DebuggerDisplay("{DebuggerDisplay,nq}")]
public abstract class CachedMemberBase<TMemberInfo>
    where TMemberInfo : MemberInfo
{
    // Fields  
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private CachedAttributes _attributes;

    // Properties 
    protected CachedMemberBase(TMemberInfo memberInfo)
    {
        ArgumentNullException.ThrowIfNull(memberInfo);
        MemberInfo = memberInfo;
    }

    // Properties
    private string DebuggerDisplay
        => Name;

    protected TMemberInfo MemberInfo { get; }

    public string Name
        => MemberInfo.Name;

    public CachedAttributes Attributes
        => _attributes ??= new CachedAttributes(MemberInfo);
}
