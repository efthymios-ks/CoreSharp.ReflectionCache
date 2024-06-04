using CoreSharp.ReflectionCache.Models.Abstracts;
using System.Reflection;

namespace CoreSharp.ReflectionCache.Models;

public sealed class CachedField : CachedMemberBase<FieldInfo>
{
    // Constructors 
    internal CachedField(FieldInfo? fieldInfo)
        : base(fieldInfo)
    {
    }

    // Properties 
    public Type Type
        => MemberInfo.FieldType;

    public bool CanWrite
        => !MemberInfo.IsInitOnly && !MemberInfo.IsLiteral;

    public TField? GetValue<TField>(object readFrom)
        => (TField?)GetValue(readFrom);

    public object? GetValue(object readFrom)
        => MemberInfo.GetValue(readFrom);

    public void SetValue(object writeTo, object value)
        => MemberInfo.SetValue(writeTo, value);
}
