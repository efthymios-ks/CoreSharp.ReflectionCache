using FluentAssertions;
using NUnit.Framework;
using System;
using Tests.Internal;

namespace CoreSharp.ReflectionCache.Models.Tests;

[TestFixture]
public sealed class CachedTypeTests
{
    [Test]
    public void Get_WithType_ShouldCreateCachedTypeInstance()
    {
        // Arrange
        var type = typeof(DummyClass);

        // Act
        var cachedType = CachedType.Get(type);

        // Assert
        cachedType.Should().NotBeNull();
        cachedType.BaseType.Should().Be(type);
        cachedType.FullName.Should().Be(type.FullName);
        cachedType.Name.Should().Be(type.Name);
    }

    [Test]
    public void Get_WithGenericType_ShouldCreateCachedTypeInstance()
    {
        // Arrange
        var type = typeof(DummyClass);

        // Act
        var cachedType = CachedType.Get<DummyClass>();

        // Assert
        cachedType.Should().NotBeNull();
        cachedType.BaseType.Should().Be(type);
        cachedType.FullName.Should().Be(type.FullName);
        cachedType.Name.Should().Be(type.Name);
    }

    [Test]
    public void Get_WithNullType_ShouldThrowArgumentNullException()
    {
        // Arrange
        Type type = null;

        // Act & Assert
        Action action = () => CachedType.Get(type);
        action.Should().Throw<ArgumentNullException>();
    }
}
