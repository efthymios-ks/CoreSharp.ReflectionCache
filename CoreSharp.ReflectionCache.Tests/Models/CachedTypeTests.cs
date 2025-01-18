using CoreSharp.ReflectionCache.Models;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace CoreSharp.ReflectionCache.Tests.Models;

public sealed class CachedTypeTests
{
    [Fact]
    public void Get_Generic_WhenCalled_ShouldReturnCachedType()
    {
        // Act 
        var cachedType = CachedType.Get<DummyClass>();

        // Assert
        Assert.NotNull(cachedType);
    }

    [Fact]
    public void Get_WhenTypeIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        Type? type = null!;

        // Act 
        void Action()
            => _ = CachedType.Get(type);

        // Assert
        Assert.Throws<ArgumentNullException>(Action);
    }

    [Fact]
    public void Get_WhenCalled_ShouldReturnCachedType()
    {
        // Arrange
        var type = typeof(DummyClass);

        // Act 
        var cachedType = CachedType.Get(type);

        // Assert
        Assert.NotNull(cachedType);
    }

    [Fact]
    public void Get_Generic_WhenCalledWithCacheDuration_ShouldReturnCachedType()
    {
        // Act 
        var cachedType = CachedType.Get<DummyClass>(TimeSpan.FromTicks(1));

        // Assert
        Assert.NotNull(cachedType);
    }

    [Fact]
    public void Get_WhenCalledWithDurationAndTypeIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        Type? type = null!;

        // Act 
        void Action()
            => _ = CachedType.Get(type, TimeSpan.FromTicks(1));

        // Assert
        Assert.Throws<ArgumentNullException>(Action);
    }

    [Fact]
    public void Get_WhenCalledWithDurationAndNotExpired_ShouldReturnSameInstance()
    {
        // Arrange
        const int durationMinutes = 1;
        var duration = TimeSpan.FromMinutes(durationMinutes);
        var firstCachedType = CachedType.Get<DummyClass>(duration);

        // Act 
        var secondCachedType = CachedType.Get<DummyClass>(duration);

        // Assert 
        Assert.Same(firstCachedType, secondCachedType);
    }

    [Fact]
    public void DebuggerDisplay_WhenCalled_ShouldReturnTypeFullName()
    {
        // Arrange
        var cachedType = CachedType.Get<DummyClass>();

        // Act 
        var debuggerDisplay = (string?)cachedType
            .GetType()
            .GetProperty("DebuggerDisplay", BindingFlags.NonPublic | BindingFlags.Instance)
            ?.GetValue(cachedType);

        // Assert
        Assert.Equal(typeof(DummyClass).FullName, debuggerDisplay);
    }

    [Fact]
    public void BaseType_WhenCalled_ShouldReturnType()
    {
        // Arrange
        var cachedType = CachedType.Get<DummyClass>();

        // Act 
        var baseType = cachedType.BaseType;

        // Assert
        Assert.Equal(typeof(DummyClass), baseType);
    }

    [Fact]
    public void FullName_WhenCalled_ShouldReturnFullName()
    {
        // Arrange
        var cachedType = CachedType.Get<DummyClass>();

        // Act 
        var fullName = cachedType.FullName;

        // Assert
        Assert.Equal(typeof(DummyClass).FullName, fullName);
    }

    [Fact]
    public void Name_WhenCalled_ShouldReturnName()
    {
        // Arrange
        var cachedType = CachedType.Get<DummyClass>();

        // Act 
        var name = cachedType.Name;

        // Assert
        Assert.Equal(nameof(DummyClass), name);
    }

    [Fact]
    public void Constructors_WhenCalled_ShouldReturnCachedConstructors()
    {
        // Arrange
        var cachedType = CachedType.Get<DummyClass>();

        // Act 
        var constructors = cachedType.Constructors;

        // Assert
        Assert.NotNull(constructors);
        Assert.Single(constructors);
        var constructor = constructors.First();
        var parameters = constructor.Parameters;
        Assert.NotNull(parameters);
        Assert.Single(parameters);
        var parameter = parameters[0];
        Assert.Equal(typeof(int), parameter.ParameterType);
    }

    [Fact]
    public void Attributes_WhenCalled_ShouldReturnCachedAttributes()
    {
        // Arrange
        var cachedType = CachedType.Get<DummyClass>();

        // Act 
        var attributes = cachedType.Attributes;

        // Assert
        Assert.NotNull(attributes);
        Assert.NotEmpty(attributes);
        var displayAttribute = attributes.FirstOrDefault(attribute => attribute is DisplayAttribute) as DisplayAttribute;
        Assert.NotNull(displayAttribute);
        Assert.Equal("DummyClass_Display", displayAttribute!.Name);
    }

    [Fact]
    public void Properties_WhenCalled_ShouldReturnCachedProperties()
    {
        // Arrange
        var cachedType = CachedType.Get<DummyClass>();

        // Act 
        var properties = cachedType.Properties;

        // Assert
        Assert.NotNull(properties);
        Assert.Single(properties);
        Assert.DoesNotContain(nameof(DummyClass.Field), properties.Keys);
        Assert.Contains(nameof(DummyClass.Property), properties.Keys);
    }

    [Fact]
    public void Fields_WhenCalled_ShouldReturnCachedFields()
    {
        // Arrange
        var cachedType = CachedType.Get<DummyClass>();

        // Act 
        var fields = cachedType.Fields;

        // Assert
        Assert.NotNull(fields);
        Assert.Single(fields);
        Assert.DoesNotContain(nameof(DummyClass.Property), fields.Keys);
        Assert.Contains(nameof(DummyClass.Field), fields.Keys);
    }

    [Fact]
    public void Methods_WhenCalled_ShouldReturnCachedMethods()
    {
        // Arrange
        var cachedType = CachedType.Get<DummyClass>();

        // Act 
        var methods = cachedType.Methods;

        // Assert
        Assert.NotNull(methods);
        Assert.NotEmpty(methods);

        var processMethod = methods.FirstOrDefault(method => method.Name == nameof(DummyClass.Process))!;
        Assert.NotNull(processMethod);
        Assert.Equal(typeof(string), processMethod.ReturnType);
        Assert.Equal(nameof(DummyClass.Process), processMethod.Name);

        var parameters = processMethod.Parameters;
        Assert.NotNull(parameters);
        Assert.Single(parameters);
        var parameter = parameters[0];
        Assert.Equal(typeof(int), parameter.ParameterType);
    }

    [Display(Name = "DummyClass_Display")]
    private sealed class DummyClass(int _)
    {
        public string? Field;

        public string? Property { get; set; }

        public static string Process(int value)
            => value.ToString();
    }
}
