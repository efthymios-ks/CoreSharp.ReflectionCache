using CoreSharp.ReflectionCache.Models.Abstracts;
using System;
using System.Diagnostics;
using System.Reflection;

namespace CoreSharp.ReflectionCache.Models;

public sealed class CachedProperty : CachedMemberBase<PropertyInfo>
{
    // Fields  
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private Delegate _getterDelegate;
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private Delegate _setterDelegate;

    public CachedProperty(PropertyInfo propertyInfo)
        : base(propertyInfo)
    {
    }

    // Properties 
    public Type Type
        => MemberInfo.PropertyType;

    public bool CanWrite
        => MemberInfo.CanWrite;

    public bool CanRead
        => MemberInfo.CanRead;

    public TProperty GetValue<TParent, TProperty>(TParent readFrom)
    {
        if (_getterDelegate is null)
        {
            var getMethod = MemberInfo.GetGetMethod(true) ?? MemberInfo.GetGetMethod();
            _getterDelegate = Delegate.CreateDelegate(typeof(Func<TParent, TProperty>), getMethod);
        }

        return ((Func<TParent, TProperty>)_getterDelegate)(readFrom);
    }

    public void SetValue<TParent, TProperty>(TParent writeTo, TProperty value)
    {
        if (_setterDelegate is null)
        {
            var setMethodInfo = MemberInfo.GetSetMethod(true) ?? MemberInfo.GetSetMethod();
            _setterDelegate = Delegate.CreateDelegate(typeof(Action<TParent, TProperty>), setMethodInfo);
        }

        ((Action<TParent, TProperty>)_setterDelegate)(writeTo, value);
    }
}
