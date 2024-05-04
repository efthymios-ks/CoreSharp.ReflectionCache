using BenchmarkDotNet.Attributes;
using CoreSharp.ReflectionCache.Models;
using System;
using System.Diagnostics.CodeAnalysis;

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

    #region Get constructors
    [Benchmark]
    public void GetConstructors_Reflection()
        => _ = _dummyType.GetConstructors();

    [Benchmark]
    public void GetConstructors_Cached()
        => _ = _dummyCachedType.Constructors;
    #endregion

    #region Get attributes
    [Benchmark]
    public void GetAttributes_Reflection()
        => _ = _dummyType.GetCustomAttributes(true);

    [Benchmark]
    public void GetAttributes_Cached()
        => _ = _dummyCachedType.Attributes;
    #endregion

    #region Get methods
    [Benchmark]
    public void GetMethods_Reflection()
        => _ = _dummyType.GetMethods();

    [Benchmark]
    public void GetMethods_Cached()
        => _ = _dummyCachedType.Methods;
    #endregion

    #region Get properties
    [Benchmark]
    public void GetProperties_Reflection()
        => _ = _dummyType.GetProperties();

    [Benchmark]
    public void GetProperties_Cached()
        => _ = _dummyCachedType.Properties;
    #endregion

    #region Get fields 
    [Benchmark]
    public void GetFields_Reflection()
        => _ = _dummyType.GetFields();

    [Benchmark]
    public void GetFields_Cached()
        => _ = _dummyCachedType.Fields;
    #endregion

    #region Get property value
    [Benchmark]
    public void GetPropertyValue_Directly()
        => _ = _dummy.Property;

    [Benchmark]
    public void GetPropertyValue_Reflection()
        => _ = _dummyType
                .GetProperty(nameof(DummyClass.Property))
                .GetValue(_dummy);

    [Benchmark]
    public void GetPropertyValue_Cached()
        => _ = _dummyCachedType
                .Properties[nameof(DummyClass.Property)]
                .GetValue<DummyClass, string>(_dummy);
    #endregion

    #region Set property value
    [Benchmark]
    public void SetPropertyValue_Directly()
        => _dummy.Property = "1";

    [Benchmark]
    public void SetPropertyValue_Reflection()
        => _dummyType
            .GetProperty(nameof(DummyClass.Property))
            .SetValue(_dummy, "1");

    [Benchmark]
    public void SetPropertyValue_Cached()
        => _dummyCachedType
            .Properties[nameof(DummyClass.Property)]
            .SetValue(_dummy, "1");
    #endregion

    #region Get field value 
    [Benchmark]
    public void GetFieldValue_Directly()
        => _ = _dummy.Field;

    [Benchmark]
    public void GetFieldValue_Reflection()
        => _ = _dummyType
                .GetField(nameof(DummyClass.Field))
                .GetValue(_dummy);

    [Benchmark]
    public void GetFieldValue_Cached()
        => _ = _dummyCachedType
                .Fields[nameof(DummyClass.Field)]
                .GetValue<string>(_dummy);
    #endregion

    #region Set field value 
    [Benchmark]
    public void SetFieldValue_Directly()
        => _dummy.Field = "1";

    [Benchmark]
    public void SetFieldValue_Reflection()
        => _dummyType
            .GetField(nameof(DummyClass.Field))
            .SetValue(_dummy, "1");

    [Benchmark]
    public void SetFieldValue_Cached()
        => _dummyCachedType
            .Fields[nameof(DummyClass.Field)]
            .SetValue(_dummy, "1");
    #endregion
}