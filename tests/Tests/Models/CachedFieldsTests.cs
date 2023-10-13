using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using Tests.Internal;

namespace CoreSharp.ReflectionCache.Models.Tests;

[TestFixture]
public sealed class CachedFieldsTests
{
    [Test]
    public void Constructor_WithValidType_ShouldInitializeFieldsDictionary()
    {
        // Arrange
        var type = typeof(DummyClass);

        // Act
        var cachedProperties = new CachedFields(type);

        // Assert
        cachedProperties.Should().NotBeNull();
        cachedProperties.Should().NotBeEmpty();
        cachedProperties.Count.Should().Be(4); // There are four fields in DummyClass
    }

    [Test]
    public void Constructor_WithNullType_ShouldInitializeEmptyFieldsDictionary()
    {
        // Arrange
        Type type = null;

        // Act
        var cachedProperties = new CachedFields(type);

        // Assert
        cachedProperties.Should().NotBeNull();
        cachedProperties.Should().BeEmpty();
    }

    [Test]
    public void Constructor_WithNoFields_ShouldInitializeEmptyFieldsDictionary()
    {
        // Arrange
        var type = typeof(DummyClassWithNoFields);

        // Act
        var cachedProperties = new CachedFields(type);

        // Assert
        cachedProperties.Should().NotBeNull();
        cachedProperties.Should().BeEmpty();
    }

    [Test]
    public void GetField_WithExistingField_ShouldReturnCachedField()
    {
        // Arrange
        var type = typeof(DummyClass);
        var cachedProperties = new CachedFields(type);
        var fieldName = nameof(DummyClass.FieldWith1Attribute);

        // Act
        var cachedField = cachedProperties[fieldName];

        // Assert
        cachedField.Should().NotBeNull();
        cachedField.Name.Should().Be(fieldName);
    }

    [Test]
    public void GetField_WithNonExistentField_ShouldReturnNull()
    {
        // Arrange
        var type = typeof(DummyClass);
        var cachedProperties = new CachedFields(type);

        // Act
        Action action = () => _ = cachedProperties["NonExistentField"];

        // Assert
        action.Should().ThrowExactly<KeyNotFoundException>();
    }
}
