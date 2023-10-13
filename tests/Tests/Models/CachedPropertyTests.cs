using FluentAssertions;
using NUnit.Framework;
using System;
using Tests.Internal;

namespace CoreSharp.ReflectionCache.Models.Tests;

[TestFixture]
public class CachedPropertyTests
{
    [Test]
    public void Type_ShouldReturnPropertyType()
    {
        // Arrange
        var propertyInfo = typeof(DummyClass).GetProperty(nameof(DummyClass.PropertyWith1Attribute));
        var cachedProperty = new CachedProperty(propertyInfo);

        // Act
        var propertyType = cachedProperty.Type;

        // Assert
        propertyType.Should().NotBeNull();
        propertyType.Should().Be(new DummyClass().PropertyWith1Attribute.GetType());
    }

    [Test]
    public void CanWrite_WithReadOnlyProperty_ShouldReturnFalse()
    {
        // Arrange
        var propertyInfo = typeof(DummyClass).GetProperty(nameof(DummyClass.ReadOnlyProperty));
        var cachedProperty = new CachedProperty(propertyInfo);

        // Act
        var canWrite = cachedProperty.CanWrite;

        // Assert
        canWrite.Should().BeFalse();
    }

    [Test]
    public void CanWrite_WithReadWriteProperty_ShouldReturnTrue()
    {
        // Arrange
        var propertyInfo = typeof(DummyClass).GetProperty(nameof(DummyClass.PropertyWith1Attribute));
        var cachedProperty = new CachedProperty(propertyInfo);

        // Act
        var canWrite = cachedProperty.CanWrite;

        // Assert
        canWrite.Should().BeTrue();
    }

    [Test]
    public void CanRead_WithReadableProperty_ShouldReturnTrue()
    {
        // Arrange
        var propertyInfo = typeof(DummyClass).GetProperty(nameof(DummyClass.PropertyWith1Attribute));
        var cachedProperty = new CachedProperty(propertyInfo);

        // Act
        var canRead = cachedProperty.CanRead;

        // Assert
        canRead.Should().BeTrue();
    }

    [Test]
    public void GetValue_WithValidValueType_ShouldReturnPropertyValue()
    {
        // Arrange 
        const string value = "value";
        var parent = new DummyClass { PropertyWith1Attribute = value };
        var propertyInfo = typeof(DummyClass).GetProperty(nameof(DummyClass.PropertyWith1Attribute));
        var cachedProperty = new CachedProperty(propertyInfo);

        // Act
        var readValue = cachedProperty.GetValue<DummyClass, string>(parent);

        // Assert
        readValue.Should().NotBeNull();
        readValue.Should().Be(value);
    }

    [Test]
    public void GetValue_WithInvalidValueType_ShouldThrowArgumentException()
    {
        // Arrange 
        var parent = new DummyClass { PropertyWith1Attribute = "value" };
        var propertyInfo = typeof(DummyClass).GetProperty(nameof(DummyClass.PropertyWith1Attribute));
        var cachedProperty = new CachedProperty(propertyInfo);

        // Act
        Action action = () => cachedProperty.GetValue<DummyClass, int>(parent);

        // Assert 
        action.Should().ThrowExactly<ArgumentException>();
    }

    [Test]
    public void GetValue_WithInvalidParent_ShouldThrowTargetException()
    {
        // Arrange 
        var parent = new DummyNonExistentAttribute();
        var propertyInfo = typeof(DummyClass).GetProperty(nameof(DummyClass.PropertyWith1Attribute));
        var cachedProperty = new CachedProperty(propertyInfo);

        // Act
        Action action = () => cachedProperty.GetValue<DummyNonExistentAttribute, string>(parent);

        // Assert
        action.Should().ThrowExactly<ArgumentException>();
    }

    [Test]
    public void GetValue_WithNullParent_ShouldThrowTargetException()
    {
        // Arrange 
        var propertyInfo = typeof(DummyClass).GetProperty(nameof(DummyClass.PropertyWith1Attribute));
        var cachedProperty = new CachedProperty(propertyInfo);

        // Act
        Action action = () => cachedProperty.GetValue<DummyClass, string>(null);

        // Assert
        action.Should().ThrowExactly<NullReferenceException>();
    }

    [Test]
    public void SetValue_WithValidValueType_ShouldSetPropertyValue()
    {
        // Arrange 
        const string value = "value";
        var parent = new DummyClass();
        var propertyInfo = typeof(DummyClass).GetProperty(nameof(DummyClass.PropertyWith1Attribute));
        var cachedProperty = new CachedProperty(propertyInfo);

        // Act
        cachedProperty.SetValue(parent, value);

        // Assert
        parent.PropertyWith1Attribute.Should().NotBeNull();
        parent.PropertyWith1Attribute.Should().Be(value);
    }

    [Test]
    public void SetValue_WithInvalidValueType_ShouldThrowArgumentException()
    {
        // Arrange   
        var parent = new DummyClass();
        var propertyInfo = typeof(DummyClass).GetProperty(nameof(DummyClass.PropertyWith1Attribute));
        var cachedProperty = new CachedProperty(propertyInfo);

        // Act
        Action action = () => cachedProperty.SetValue(parent, 0);

        // Assert
        action.Should().ThrowExactly<ArgumentException>();
    }

    [Test]
    public void SetValue_WithInvalidParent_ShouldThrowTargetException()
    {
        // Arrange 
        const string value = "value";
        var parent = new DummyNonExistentAttribute();
        var propertyInfo = typeof(DummyClass).GetProperty(nameof(DummyClass.PropertyWith1Attribute));
        var cachedProperty = new CachedProperty(propertyInfo);

        // Act
        Action action = () => cachedProperty.SetValue(parent, value);

        // Assert
        action.Should().ThrowExactly<ArgumentException>();
    }

    [Test]
    public void SetValue_WithNullParent_ShouldThrowTargetException()
    {
        // Arrange 
        const string value = "value";
        DummyClass parent = null;
        var propertyInfo = typeof(DummyClass).GetProperty(nameof(DummyClass.PropertyWith1Attribute));
        var cachedProperty = new CachedProperty(propertyInfo);

        // Act
        Action action = () => cachedProperty.SetValue(parent, value);

        // Assert 
        action.Should().ThrowExactly<NullReferenceException>();
    }
}