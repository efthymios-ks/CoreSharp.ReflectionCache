using CoreSharp.ReflectionCache.Models.Abstracts;
using System.Diagnostics;
using System.Reflection;

namespace CoreSharp.ReflectionCache.Models;

public sealed class CachedProperty(PropertyInfo? propertyInfo)
    : CachedMemberBase<PropertyInfo>(propertyInfo)
{
    // Fields  
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private Delegate? _getterDelegate;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private Delegate? _setterDelegate;

    // Properties 
    public Type Type
        => MemberInfo.PropertyType;

    public bool CanWrite
        => MemberInfo.CanWrite;

    public bool CanRead
        => MemberInfo.CanRead;

    public object? GetValue<TParent>(TParent readFrom)
    {
        if (_getterDelegate is null)
        {
            var getterMethodInfo = MemberInfo.GetGetMethod()
                ?? MemberInfo.GetGetMethod(true)
                ?? throw new InvalidOperationException($"Could not found getter for '{MemberInfo.DeclaringType!.FullName}' > '{MemberInfo.Name}'.");
            _getterDelegate = Delegate.CreateDelegate(typeof(Func<TParent, object?>), getterMethodInfo!);
        }

        return ((Func<TParent, object?>)_getterDelegate)(readFrom);
    }

    public void SetValue<TParent, TProperty>(TParent writeTo, TProperty value)
    {
        if (_setterDelegate is null)
        {
            var setterMethodInfo = MemberInfo.GetSetMethod()
                ?? MemberInfo.GetSetMethod(true)
                ?? throw new InvalidOperationException($"Could not found setter for '{MemberInfo.DeclaringType!.FullName}' > '{MemberInfo.Name}'.");
            _setterDelegate = Delegate.CreateDelegate(typeof(Action<TParent, TProperty>), setterMethodInfo!);
        }

        ((Action<TParent, TProperty>)_setterDelegate)(writeTo, value);
    }
}
