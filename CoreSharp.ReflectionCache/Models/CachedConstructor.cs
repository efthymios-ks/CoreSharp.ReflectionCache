using CoreSharp.ReflectionCache.Models.Abstracts;
using System.Reflection;

namespace CoreSharp.ReflectionCache.Models;

public sealed class CachedConstructor : CachedMemberBase<ConstructorInfo>
{
    // Fields
    private ParameterInfo[]? _parameters;

    // Constructors
    internal CachedConstructor(ConstructorInfo? memberInfo)
        : base(memberInfo)
    {
    }

    // Properties 
    public ParameterInfo[] Parameters
        => _parameters ??= MemberInfo.GetParameters();

    // Methods
    public TResult Invoke<TResult>(params object[] parameters)
        => (TResult)Invoke(parameters);

    public object Invoke(params object[] parameters)
        => MemberInfo.Invoke(parameters);
}
