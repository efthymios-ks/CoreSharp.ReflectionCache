using CoreSharp.ReflectionCache.Models.Abstracts;
using System;
using System.Reflection;

namespace CoreSharp.ReflectionCache.Models;

public sealed class CachedMethod : CachedMemberBase<MethodInfo>
{
    private ParameterInfo[] _parameters;

    internal CachedMethod(MethodInfo methodInfo)
        : base(methodInfo)
    {
    }

    // Properties 
    public Type ReturnType
        => MemberInfo.ReturnType;

    public ParameterInfo[] Parameters
        => _parameters ??= MemberInfo.GetParameters();

    // Methods
    public TResult Invoke<TResult>(object parent, params object[] parameters)
        => (TResult)Invoke(parent, parameters);

    public object Invoke(object parent, params object[] parameters)
        => MemberInfo.Invoke(parent, parameters);
}
