using CoreSharp.ReflectionCache.Models;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Tests.Models;

[TestFixture]
public class CachedPropertyTests
{
    [Test]
    public void Constructor_WhenPropertyInfoIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        PropertyInfo propertyInfo = null!;

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
        var valueRead = cachedProperty.GetValue<DummyClass, string>(parent);

        // Assert
        valueRead.Should().NotBeNull();
        valueRead.Should().Be(parent.Property);
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

    private sealed class DummyClass
    {
        [SuppressMessage("CodeQuality", "IDE0052:Remove unread private members",
            Justification = "<Pending>")]
        private static string _writeOnlyProperty;

        public string Property { get; set; } = "Property";

        public static string ReadOnlyProperty
            => "ReadOnlyProperty";

        [SuppressMessage("Major Code Smell", "S2376:Write-only properties should not be used",
            Justification = "<Pending>")]
        public static string WriteOnlyProperty
        {
            set => _writeOnlyProperty = value;
        }
    }
}