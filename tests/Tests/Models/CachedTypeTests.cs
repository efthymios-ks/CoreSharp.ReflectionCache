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
    public void GetGeneric_WhenCalled_ShouldReturnCachedType()
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
    public void GetGeneric_WhenCalledWithCacheDuration_ShouldReturnCachedType()
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

    [Display(Name = "DummyClass_Display")]
    private sealed class DummyClass
    {
        public string Field;

        public string Property { get; set; }
    }
}
