﻿using CoreSharp.ReflectionCache.Models;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using System.Reflection;

namespace Tests.Models;

[TestFixture]
public class CachedPropertyTests
{
    [Test]
    public void Constructor_WhenPropertyInfoIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        PropertyInfo? propertyInfo = null;

        // Act
        Action action = () => _ = new CachedProperty(propertyInfo);

        // Assert
        action.Should().ThrowExactly<ArgumentNullException>();
    }

    [Test]
    public void Constructor_WhenPropertyInfoIsNotNull_ShouldNotThrowException()
    {
        // Arrange
        var propertyInfo = typeof(DummyClass)
            .GetProperty(nameof(DummyClass.Property));

        // Act
        Action action = () => _ = new CachedProperty(propertyInfo);

        // Assert
        action.Should().NotThrow();
    }

    [Test]
    public void Type_WhenCalled_ShouldReturnFieldType()
    {
        // Arrange
        var propertyInfo = typeof(DummyClass)
            .GetProperty(nameof(DummyClass.Property));

        var cachedProperty = new CachedProperty(propertyInfo);

        // Act
        var propertyType = cachedProperty.Type;

        // Assert
        propertyType.Should().NotBeNull();
        propertyType.Should().Be(typeof(string));
    }

    [Test]
    public void CanWrite_WhenPropertyIsReadOnly_ShouldReturnFalse()
    {
        // Arrange
        var propertyInfo = typeof(DummyClass)
            .GetProperty(nameof(DummyClass.ReadOnlyProperty));
        var cachedProperty = new CachedProperty(propertyInfo);

        // Act
        var canWrite = cachedProperty.CanWrite;

        // Assert
        canWrite.Should().BeFalse();
    }

    [Test]
    public void CanWrite_WhenFieldIsWritable_ShouldReturnTrue()
    {
        // Arrange
        var propertyInfo = typeof(DummyClass)
            .GetProperty(nameof(DummyClass.Property));
        var cachedProperty = new CachedProperty(propertyInfo);

        // Act
        var canWrite = cachedProperty.CanWrite;

        // Assert
        canWrite.Should().BeTrue();
    }

    [Test]
    public void CanRead_WhenPropertyIsWriteOnly_ShouldReturnFalse()
    {
        // Arrange
        var propertyInfo = typeof(DummyClass)
            .GetProperty(nameof(DummyClass.WriteOnlyProperty));
        var cachedProperty = new CachedProperty(propertyInfo);

        // Act
        var canWrite = cachedProperty.CanRead;

        // Assert
        canWrite.Should().BeFalse();
    }

    [Test]
    public void CanRead_WhenFieldIsReadable_ShouldReturnTrue()
    {
        // Arrange
        var propertyInfo = typeof(DummyClass)
            .GetProperty(nameof(DummyClass.Property));
        var cachedProperty = new CachedProperty(propertyInfo);

        // Act
        var canWrite = cachedProperty.CanRead;

        // Assert
        canWrite.Should().BeTrue();
    }

    [Test]
    public void GetValue_WhenCalled_ShouldReturnFieldValue()
    {
        // Arrange 
        var parent = new DummyClass()
        {
            Property = "Value"
        };
        var propertyInfo = typeof(DummyClass)
            .GetProperty(nameof(DummyClass.Property));
        var cachedProperty = new CachedProperty(propertyInfo);

        // Act
        var valueRead = cachedProperty.GetValue(parent);

        // Assert
        valueRead.Should().NotBeNull();
        valueRead.Should().Be(parent.Property);
    }

    [Test]
    public void GetValue_WhenGetterIsNotPublic_ShouldReturnPrivateGetter()
    {
        // Arrange
        var propertyInfo = Substitute.For<PropertyInfo>();
        var cachedProperty = new CachedProperty(propertyInfo);

        var getterMethodInfoMock = Substitute.For<MethodInfo>();
        propertyInfo
            .GetGetMethod(true)
            .Returns(getterMethodInfoMock);

        // Act 
        Action action = () => _ = cachedProperty.GetValue(new object());

        // Assert 
        // Could not find any other way to mock MethodInfo.
        // So I just ignore the error that follows after my test case.
        action.Should()
            .ThrowExactly<ArgumentException>()
            .WithMessage("MethodInfo must be a runtime MethodInfo object. (Parameter 'method')");

        propertyInfo
          .GetGetMethod(true)
          .Received(1);
    }

    [Test]
    public void GetValue_WhenNoGetterIsFound_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var propertyInfo = Substitute.For<PropertyInfo>();
        var cachedProperty = new CachedProperty(propertyInfo);

        var parent = new DummyClass();
        propertyInfo
            .DeclaringType
            .Returns(parent.GetType());

        propertyInfo
            .Name
            .Returns(nameof(DummyClass.Property));

        // Act 
        Action action = () => _ = cachedProperty.GetValue(parent);

        // Assert 
        action.Should().ThrowExactly<InvalidOperationException>();
    }

    [Test]
    public void SetValue_WhenCalled_ShouldSetValue()
    {
        // Arrange 
        const string valueToSet = "Value";
        var parent = new DummyClass();
        var propertyInfo = typeof(DummyClass)
            .GetProperty(nameof(DummyClass.Property));
        var cacheProperty = new CachedProperty(propertyInfo);

        // Act
        cacheProperty.SetValue(parent, valueToSet);

        // Assert 
        parent.Property.Should().Be(valueToSet);
    }

    [Test]
    public void SetValue_WhenGetterIsNotPublic_ShouldReturnPrivateGetter()
    {
        // Arrange
        var propertyInfo = Substitute.For<PropertyInfo>();
        var cachedProperty = new CachedProperty(propertyInfo);

        var getterMethodInfoMock = Substitute.For<MethodInfo>();
        propertyInfo
            .GetSetMethod(true)
            .Returns(getterMethodInfoMock);

        // Act 
        Action action = () => cachedProperty.SetValue(new object(), "");

        // Assert 
        // Could not find any other way to mock MethodInfo.
        // So I just ignore the error that follows after my test case.
        action.Should()
            .ThrowExactly<ArgumentException>()
            .WithMessage("MethodInfo must be a runtime MethodInfo object. (Parameter 'method')");

        propertyInfo
          .GetSetMethod(true)
          .Received(1);
    }

    [Test]
    public void SetValue_WhenNoGetterIsFound_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var propertyInfo = Substitute.For<PropertyInfo>();
        var cachedProperty = new CachedProperty(propertyInfo);

        var parent = new DummyClass();
        propertyInfo
            .DeclaringType
            .Returns(parent.GetType());

        propertyInfo
            .Name
            .Returns(nameof(DummyClass.Property));

        // Act 
        Action action = () => cachedProperty.SetValue(parent, "");

        // Assert 
        action.Should().ThrowExactly<InvalidOperationException>();
    }

    private sealed class DummyClass
    {
        private static string? _writeOnlyProperty;

        public string Property { get; set; } = "Property";

        public static string ReadOnlyProperty
            => "ReadOnlyProperty";

        public static string WriteOnlyProperty
        {
            set => _writeOnlyProperty = value;
        }
    }
}
