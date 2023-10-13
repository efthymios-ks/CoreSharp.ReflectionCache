using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using Tests.Internal;

namespace CoreSharp.ReflectionCache.Models.Tests;

[TestFixture]
public sealed class CachedPropertiesTests
{
    [Test]
    public void Constructor_WithValidType_ShouldInitializePropertiesDictionary()
    {
        // Arrange
        var type = typeof(DummyClass);

        // Act
        var cachedProperties = new CachedProperties(type);

        // Assert
        cachedProperties.Should().NotBeNull();
        cachedProperties.Should().NotBeEmpty();
        cachedProperties.Count.Should().Be(3); // There are three properties in DummyClass
    }

    [Test]
    public void Constructor_WithNullType_ShouldInitializeEmptyPropertiessDictionary()
    {
        // Arrange
        Type type = null;

        // Act
        var cachedProperties = new CachedProperties(type);

        // Assert
        cachedProperties.Should().NotBeNull();
        cachedProperties.Should().BeEmpty();
    }

    [Test]
    public void Constructor_WithNoProperties_ShouldInitializeEmptyPropertiesDictionary()
    {
        // Arrange
        var type = typeof(DummyClassWithNoProperties);

        // Act
        var cachedProperties = new CachedProperties(type);

        // Assert
        cachedProperties.Should().NotBeNull();
        cachedProperties.Should().BeEmpty();
    }

    [Test]
    public void GetProperty_WithExistingProperty_ShouldReturnCachedProperty()
    {
        // Arrange
        var type = typeof(DummyClass);
        var cachedProperties = new CachedProperties(type);
        var propertyName = nameof(DummyClass.PropertyWith1Attribute);

        // Act
        var cachedField = cachedProperties[propertyName];

        // Assert
        cachedField.Should().NotBeNull();
        cachedField.Name.Should().Be(propertyName);
    }

    [Test]
    public void GetProperty_WithNonExistingProperty_ShouldReturnNull()
    {
        // Arrange
        var type = typeof(DummyClass);
        var cachedProperties = new CachedProperties(type);

        // Act
        Action action = () => _ = cachedProperties["NonExistingProperty"];

        // Assert
        action.Should().ThrowExactly<KeyNotFoundException>();
    }
}
