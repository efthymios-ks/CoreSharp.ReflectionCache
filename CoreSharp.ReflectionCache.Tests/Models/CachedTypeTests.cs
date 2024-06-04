using CoreSharp.ReflectionCache.Models;
using FluentAssertions;
using NUnit.Framework;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Tests.Models;

[TestFixture]
public sealed class CachedTypeTests
{
    [Test]
    public void Get_Generic_WhenCalled_ShouldReturnCachedType()
    {
        // Act 
        var cachedType = CachedType.Get<DummyClass>();

        // Assert
        _ = cachedType.Should().NotBeNull();
    }

    [Test]
    public void Get_WhenTypeIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        Type? type = null;

        // Act 
        Action action = () => _ = CachedType.Get(type);

        // Assert
        _ = action.Should().ThrowExactly<ArgumentNullException>();
    }

    [Test]
    public void Get_WhenCalled_ShouldReturnCachedType()
    {
        // Arrange
        var type = typeof(DummyClass);

        // Act 
        var cachedType = CachedType.Get(type);

        // Assert
        _ = cachedType.Should().NotBeNull();
    }

    [Test]
    public void Get_Generic_WhenCalledWithCacheDuration_ShouldReturnCachedType()
    {
        // Act 
        var cachedType = CachedType.Get<DummyClass>(TimeSpan.FromTicks(1));

        // Assert
        _ = cachedType.Should().NotBeNull();
    }

    [Test]
    public void Get_WhenCalledWithDurationTypeIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        Type? type = null;

        // Act 
        Action action = () => _ = CachedType.Get(type, TimeSpan.FromTicks(1));

        // Assert
        _ = action.Should().ThrowExactly<ArgumentNullException>();
    }

    [Test]
    public void Get_WhenCalledWithDurationAndNotExpired_ShouldReturnSameInstance()
    {
        // Arrange
        var duration = TimeSpan.FromMinutes(1);
        var firstCachedType = CachedType.Get<DummyClass>(duration);

        // Act 
        var secondCachedType = CachedType.Get<DummyClass>(duration);

        // Assert 
        _ = secondCachedType.Should().BeSameAs(firstCachedType);
    }

    [Test]
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
        _ = debuggerDisplay.Should().Be(typeof(DummyClass).FullName);
    }

    [Test]
    public void BaseType_WhenCalled_ShouldReturnType()
    {
        // Arrange
        var cachedType = CachedType.Get<DummyClass>();

        // Act 
        var baseType = cachedType.BaseType;

        // Assert
        _ = baseType.Should().Be(typeof(DummyClass));
    }

    [Test]
    public void FullName_WhenCalled_ShouldReturnFullName()
    {
        // Arrange
        var cachedType = CachedType.Get<DummyClass>();

        // Act 
        var fullName = cachedType.FullName;

        // Assert
        _ = fullName.Should().Be(typeof(DummyClass).FullName);
    }

    [Test]
    public void Name_WhenCalled_ShouldReturnName()
    {
        // Arrange
        var cachedType = CachedType.Get<DummyClass>();

        // Act 
        var name = cachedType.Name;

        // Assert
        _ = name.Should().Be(nameof(DummyClass));
    }

    [Test]
    public void Constructors_WhenCalled_ShouldReturnCachedConstructors()
    {
        // Arrange
        var cachedType = CachedType.Get<DummyClass>();

        // Act 
        var constructors = cachedType.Constructors;

        // Assert
        _ = constructors.Should().NotBeNull();
        _ = constructors.Should().HaveCount(1);
        var constructor = constructors.First();
        var parameters = constructor.Parameters;
        _ = constructors.Should().NotBeNull();
        _ = constructors.Should().HaveCount(1);
        var parameter = parameters[0];
        _ = parameter.ParameterType.Should().Be(typeof(int));
    }

    [Test]
    public void Attributes_WhenCalled_ShouldReturnCachedAttributes()
    {
        // Arrange
        var cachedType = CachedType.Get<DummyClass>();

        // Act 
        var attributes = cachedType.Attributes;

        // Assert
        _ = attributes.Should().NotBeNull();
        _ = attributes.Should().HaveCountGreaterThan(0);
        var displayAttribute = attributes.FirstOrDefault(attribute => attribute is DisplayAttribute) as DisplayAttribute;
        _ = displayAttribute.Should().NotBeNull();
        _ = displayAttribute!.Name.Should().Be("DummyClass_Display");
    }

    [Test]
    public void Properties_WhenCalled_ShouldReturnCachedProperties()
    {
        // Arrange
        var cachedType = CachedType.Get<DummyClass>();

        // Act 
        var properties = cachedType.Properties;

        // Assert
        _ = properties.Should().NotBeNull();
        _ = properties.Should().HaveCount(1);
        _ = properties.Should().NotContainKey(nameof(DummyClass.Field));
        _ = properties.Should().ContainKey(nameof(DummyClass.Property));
    }

    [Test]
    public void Fields_WhenCalled_ShouldReturnCachedFields()
    {
        // Arrange
        var cachedType = CachedType.Get<DummyClass>();

        // Act 
        var fields = cachedType.Fields;

        // Assert
        _ = fields.Should().NotBeNull();
        _ = fields.Should().HaveCount(1);
        _ = fields.Should().NotContainKey(nameof(DummyClass.Property));
        _ = fields.Should().ContainKey(nameof(DummyClass.Field));
    }

    [Test]
    public void Methods_WhenCalled_ShouldReturnCachedMethods()
    {
        // Arrange
        var cachedType = CachedType.Get<DummyClass>();

        // Act 
        var methods = cachedType.Methods;

        // Assert
        _ = methods.Should().NotBeNull();
        _ = methods.Should().NotBeEmpty();

        var processMethod = methods.FirstOrDefault(method => method.Name == nameof(DummyClass.Process))!;
        _ = processMethod.Should().NotBeNull();
        _ = processMethod.ReturnType.Should().Be(typeof(string));
        _ = processMethod.Name.Should().Be(nameof(DummyClass.Process));

        var parameters = processMethod.Parameters;
        _ = parameters.Should().NotBeNull();
        _ = parameters.Should().HaveCount(1);
        var parameter = parameters[0];
        _ = parameter.ParameterType.Should().Be(typeof(int));
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
