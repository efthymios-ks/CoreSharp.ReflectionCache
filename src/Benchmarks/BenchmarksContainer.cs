using BenchmarkDotNet.Attributes;
using CoreSharp.ReflectionCache.Models;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Benchmarks;

[SuppressMessage("Critical Code Smell", "S1186:Methods should not be empty", Justification = "<Pending>")]
[SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "<Pending>")]
[SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores", Justification = "<Pending>")]
[Config(typeof(BenchmarksContainerConfig))]
public partial class BenchmarksContainer
{
    // Fields
    private DummyClass _dummy;
    private Type _dummyType;
    private CachedType _dummyCachedType;

    // Methods 
    [GlobalSetup]
    public void GlobalSetup()
    {
        _dummy = new DummyClass();
        _dummyType = typeof(DummyClass);
        _dummyCachedType = CachedType.Get<DummyClass>();
    }

    [GlobalCleanup]
    public void GlobalCleanup()
    {
    }

    #region Get attributes
    [Benchmark]
    public void Reflection_GetAttributes()
        => _ = _dummyType.GetCustomAttributes(true);

    [Benchmark]
    public void CachedReflection_GetAttributes()
        => _ = _dummyCachedType.Attributes;
    #endregion

    #region Get properties
    [Benchmark]
    public void Reflection_GetProperties()
        => _ = _dummyType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

    [Benchmark]
    public void CachedReflection_GetProperties()
        => _ = _dummyCachedType.Properties;
    #endregion

    #region Get fields 
    [Benchmark]
    public void Reflection_GetFields()
        => _ = _dummyType.GetFields(BindingFlags.Public | BindingFlags.Instance);

    [Benchmark]
    public void CachedReflection_GetFields()
        => _ = _dummyCachedType.Fields;
    #endregion

    #region Get property value
    [Benchmark]
    public void Direct_GetPropertyValue()
        => _ = _dummy.PropertyWith1Attribute;

    [Benchmark]
    public void Reflection_GetPropertyValue()
        => _ = _dummyType
                .GetProperty(nameof(DummyClass.PropertyWith1Attribute))
                .GetValue(_dummy);

    [Benchmark]
    public void CachedReflection_GetPropertyValue()
        => _ = _dummyCachedType
                .Properties[nameof(DummyClass.PropertyWith1Attribute)]
                .GetValue<DummyClass, string>(_dummy);
    #endregion

    #region Set property value
    [Benchmark]
    public void Direct_SetPropertyValue()
        => _dummy.PropertyWith1Attribute = "1";

    [Benchmark]
    public void Reflection_SetPropertyValue()
        => _dummyType
            .GetProperty(nameof(DummyClass.PropertyWith1Attribute))
            .SetValue(_dummy, "1");

    [Benchmark]
    public void CachedReflection_SetPropertyValue()
        => _dummyCachedType
            .Properties[nameof(DummyClass.PropertyWith1Attribute)]
            .SetValue(_dummy, "1");
    #endregion

    #region Get field value 
    [Benchmark]
    public void Direct_GetFieldValue()
        => _ = _dummy.FieldWith1Attribute;

    [Benchmark]
    public void Reflection_GetFieldValue()
        => _ = _dummyType
                .GetField(nameof(DummyClass.FieldWith1Attribute))
                .GetValue(_dummy);

    [Benchmark]
    public void CachedReflection_GetFieldValue()
        => _ = _dummyCachedType
                .Fields[nameof(DummyClass.FieldWith1Attribute)]
                .GetValue<string>(_dummy);
    #endregion

    #region Set field value 
    [Benchmark]
    public void Direct_SetFieldValue()
        => _dummy.FieldWith1Attribute = "1";

    [Benchmark]
    public void Reflection_SetFieldValue()
        => _dummyType
            .GetField(nameof(DummyClass.FieldWith1Attribute))
            .SetValue(_dummy, "1");

    [Benchmark]
    public void CachedReflection_SetFieldValue()
        => _dummyCachedType
            .Fields[nameof(DummyClass.FieldWith1Attribute)]
            .SetValue(_dummy, "1");
    #endregion
}