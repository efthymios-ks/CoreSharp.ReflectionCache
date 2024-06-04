using BenchmarkDotNet.Attributes;
using CoreSharp.ReflectionCache.Models;
using System.Reflection;

namespace Benchmarks;

[Config(typeof(BenchmarksConfig))]
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

    // Get constructors
    [BenchmarkCategory("Get constructors")]
    [Benchmark(Description = "Reflection", Baseline = true)]
    public ConstructorInfo[] GetConstructorsReflection()
    {
        var constructors = _dummyType.GetConstructors();
        return constructors;
    }

    [BenchmarkCategory("Get constructors")]
    [Benchmark(Description = "Cached")]
    public CachedConstructors GetConstructorsCached()
    {
        var constructors = _dummyCachedType.Constructors;
        return constructors;
    }

    // Get attributes
    [BenchmarkCategory("Get attributes")]
    [Benchmark(Description = "Reflection", Baseline = true)]
    public object[] GetAttributesReflection()
    {
        var attributes = _dummyType.GetCustomAttributes(true);
        return attributes;
    }

    [BenchmarkCategory("Get attributes")]
    [Benchmark(Description = "Cached")]
    public CachedAttributes GetAttributesCached()
    {
        var attributes = _dummyCachedType.Attributes;
        return attributes;
    }

    // Get methods
    [BenchmarkCategory("Get methods")]
    [Benchmark(Description = "Reflection", Baseline = true)]
    public MethodInfo[] GetMethodsReflection()
    {
        var methods = _dummyType.GetMethods();
        return methods;
    }

    [BenchmarkCategory("Get methods")]
    [Benchmark(Description = "Cached")]
    public CachedMethods GetMethodsCached()
    {
        var methods = _dummyCachedType.Methods;
        return methods;
    }

    // Get properties
    [BenchmarkCategory("Get properties")]
    [Benchmark(Description = "Reflection", Baseline = true)]
    public PropertyInfo[] GetPropertiesReflection()
    {
        var properties = _dummyType.GetProperties();
        return properties;
    }

    [BenchmarkCategory("Get properties")]
    [Benchmark(Description = "Cached")]
    public CachedProperties GetPropertiesCached()
    {
        var properties = _dummyCachedType.Properties;
        return properties;
    }

    // Get fields 
    [BenchmarkCategory("Get fields")]
    [Benchmark(Description = "Reflection", Baseline = true)]
    public FieldInfo[] GetFieldsReflection()
    {
        var fields = _dummyType.GetFields();
        return fields;
    }

    [BenchmarkCategory("Get fields")]
    [Benchmark(Description = "Cached")]
    public CachedFields GetFieldsCached()
    {
        var fields = _dummyCachedType.Fields;
        return fields;
    }

    // Get property value 
    [BenchmarkCategory("Get property value")]
    [Benchmark(Description = "Reflection", Baseline = true)]
    public object GetPropertyValueReflection()
    {
        var value = _dummyType
            .GetProperty(nameof(DummyClass.Property))
            .GetValue(_dummy);
        return value;
    }

    [BenchmarkCategory("Get property value")]
    [Benchmark(Description = "Cached")]
    public object GetPropertyValueCached()
    {
        var value = _dummyCachedType
            .Properties[nameof(DummyClass.Property)]
            .GetValue(_dummy);
        return value;
    }

    // Set property value 
    [BenchmarkCategory("Set property value")]
    [Benchmark(Description = "Reflection", Baseline = true)]
    public void SetPropertyValueReflection()
        => _dummyType
            .GetProperty(nameof(DummyClass.Property))
            .SetValue(_dummy, "1");

    [BenchmarkCategory("Set property value")]
    [Benchmark(Description = "Cached")]
    public void SetPropertyValueCached()
        => _dummyCachedType
            .Properties[nameof(DummyClass.Property)]
            .SetValue(_dummy, "1");

    // Get field value  
    [BenchmarkCategory("Get field value")]
    [Benchmark(Description = "Reflection", Baseline = true)]
    public object GetFieldValueReflection()
    {
        var value = _ = _dummyType
            .GetField(nameof(DummyClass.Field))
            .GetValue(_dummy);
        return value;
    }

    [BenchmarkCategory("Get field value")]
    [Benchmark(Description = "Cached")]
    public string GetFieldValueCached()
    {
        var value = _dummyCachedType
            .Fields[nameof(DummyClass.Field)]
            .GetValue<string>(_dummy);
        return value;
    }

    // Set field value  
    [BenchmarkCategory("Set field value")]
    [Benchmark(Description = "Reflection", Baseline = true)]
    public void SetFieldValueReflection()
        => _dummyType
            .GetField(nameof(DummyClass.Field))
            .SetValue(_dummy, "1");

    [BenchmarkCategory("Set field value")]
    [Benchmark(Description = "Cached")]
    public void SetFieldValueCached()
        => _dummyCachedType
            .Fields[nameof(DummyClass.Field)]
            .SetValue(_dummy, "1");
}
