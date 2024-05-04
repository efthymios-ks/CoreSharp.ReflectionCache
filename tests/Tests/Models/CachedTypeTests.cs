using CoreSharp.ReflectionCache.Models;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
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
        cachedType.Should().NotBeNull();
    }

    [Test]
    public void Get_WhenTypeIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        Type type = null;

        // Act 
        Action action = () => _ = CachedType.Get(type);

        // Assert
        action.Should().ThrowExactly<ArgumentNullException>();
    }

    [Test]
    public void Get_WhenCalled_ShouldReturnCachedType()
    {
        // Arrange
        var type = typeof(DummyClass);

        // Act 
        var cachedType = CachedType.Get(type);

        // Assert
        cachedType.Should().NotBeNull();
    }

    [Test]
    public void Get_Generic_WhenCalledWithCacheDuration_ShouldReturnCachedType()
    {
        // Act 
        var cachedType = CachedType.Get<DummyClass>(TimeSpan.FromTicks(1));

        // Assert
        cachedType.Should().NotBeNull();
    }

    [Test]
    public void Get_WhenCalledWithDurationTypeIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        Type type = null;

        // Act 
        Action action = () => _ = CachedType.Get(type, TimeSpan.FromTicks(1));

        // Assert
        action.Should().ThrowExactly<ArgumentNullException>();
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
        secondCachedType.Should().BeSameAs(firstCachedType);
    }

    [Test]
    public void DebuggerDisplay_WhenCalled_ShouldReturnTypeFullName()
    {
        // Arrange
        var cachedType = CachedType.Get<DummyClass>();

        // Act 
        var debuggerDisplay = (string)cachedType
            .GetType()
            .GetProperty("DebuggerDisplay", BindingFlags.NonPublic | BindingFlags.Instance)
            .GetValue(cachedType);

        // Assert
        debuggerDisplay.Should().Be(typeof(DummyClass).FullName);
    }

    [Test]
    public void BaseType_WhenCalled_ShouldReturnType()
    {
        // Arrange
        var cachedType = CachedType.Get<DummyClass>();

        // Act 
        var baseType = cachedType.BaseType;

        // Assert
        baseType.Should().Be(typeof(DummyClass));
    }

    [Test]
    public void FullName_WhenCalled_ShouldReturnFullName()
    {
        // Arrange
        var cachedType = CachedType.Get<DummyClass>();

        // Act 
        var fullName = cachedType.FullName;

        // Assert
        fullName.Should().Be(typeof(DummyClass).FullName);
    }

    [Test]
    public void Name_WhenCalled_ShouldReturnName()
    {
        // Arrange
        var cachedType = CachedType.Get<DummyClass>();

        // Act 
        var name = cachedType.Name;

        // Assert
        name.Should().Be(nameof(DummyClass));
    }

    [Test]
    public void Constructors_WhenCalled_ShouldReturnCachedConstructors()
    {
        // Arrange
        var cachedType = CachedType.Get<DummyClass>();

        // Act 
        var constructors = cachedType.Constructors;

        // Assert
        constructors.Should().NotBeNull();
        constructors.Should().HaveCount(1);
        var constructor = constructors.First();
        var parameters = constructor.Parameters;
        constructors.Should().NotBeNull();
        constructors.Should().HaveCount(1);
        var parameter = parameters[0];
        parameter.ParameterType.Should().Be(typeof(int));
    }

    [Test]
    public void Attributes_WhenCalled_ShouldReturnCachedAttributes()
    {
        // Arrange
        var cachedType = CachedType.Get<DummyClass>();

        // Act 
        var attributes = cachedType.Attributes;

        // Assert
        attributes.Should().NotBeNull();
        attributes.Should().HaveCount(1);
        var displayAttribute = attributes.First().Should().BeOfType<DisplayAttribute>().Subject;
        displayAttribute.Name.Should().Be("DummyClass_Display");
    }

    [Test]
    public void Properties_WhenCalled_ShouldReturnCachedProperties()
    {
        // Arrange
        var cachedType = CachedType.Get<DummyClass>();

        // Act 
        var properties = cachedType.Properties;

        // Assert
        properties.Should().NotBeNull();
        properties.Should().HaveCount(1);
        properties.Should().NotContainKey(nameof(DummyClass.Field));
        properties.Should().ContainKey(nameof(DummyClass.Property));
    }

    [Test]
    public void Fields_WhenCalled_ShouldReturnCachedFields()
    {
        // Arrange
        var cachedType = CachedType.Get<DummyClass>();

        // Act 
        var fields = cachedType.Fields;

        // Assert
        fields.Should().NotBeNull();
        fields.Should().HaveCount(1);
        fields.Should().NotContainKey(nameof(DummyClass.Property));
        fields.Should().ContainKey(nameof(DummyClass.Field));
    }

    [Test]
    public void Methods_WhenCalled_ShouldReturnCachedMethods()
    {
        // Arrange
        var cachedType = CachedType.Get<DummyClass>();

        // Act 
        var methods = cachedType.Methods;

        // Assert
        methods.Should().NotBeNull();
        methods.Should().NotBeEmpty();

        var processMethod = methods.FirstOrDefault(method => method.Name == nameof(DummyClass.Process));
        processMethod.Should().NotBeNull();
        processMethod.ReturnType.Should().Be(typeof(string));
        processMethod.Name.Should().Be(nameof(DummyClass.Process));

        var parameters = processMethod.Parameters;
        parameters.Should().NotBeNull();
        parameters.Should().HaveCount(1);
        var parameter = parameters[0];
        parameter.ParameterType.Should().Be(typeof(int));
    }

    [Display(Name = "DummyClass_Display")]
    private sealed class DummyClass
    {
        public string Field;

        public DummyClass(int _)
        {
        }

        public string Property { get; set; }
        public static string Process(int value)
            => value.ToString();
    }
}
