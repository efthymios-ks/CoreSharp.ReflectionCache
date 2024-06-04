using CoreSharp.ReflectionCache.Models;
using FluentAssertions;
using NUnit.Framework;

namespace Tests.Models;

[TestFixture]
public sealed class CachedPropertiesTests
{
    [Test]
    public void Constructor_WhenTypeIsNull_ShouldNotThrowException()
    {
        // Arrange
        Type? type = null;

        // Act
        Action action = () => _ = new CachedProperties(type);

        // Assert
        action.Should().NotThrow();
    }

    [Test]
    public void Constructor_WhenTypeIsNull_ShouldInitializeAsEmpty()
    {
        // Arrange
        Type? type = null;

        // Act
        var cachedProperties = new CachedProperties(type);

        // Assert
        cachedProperties.Should().NotBeNull();
        cachedProperties.Should().BeEmpty();
    }

    [Test]
    public void Constructor_WhenCalled_ShouldInitializeFields()
    {
        // Arrange
        var type = typeof(DummyClass);

        // Act
        var cachedProperties = new CachedProperties(type);

        // Assert
        cachedProperties.Should().NotBeNull();
        cachedProperties.Should().HaveCount(1);
        cachedProperties.Should().NotContainKey(nameof(DummyClass.Field1));
        cachedProperties.Should().ContainKey(nameof(DummyClass.Property1));
    }

    private sealed class DummyClass
    {
        public string? Field1;
        public string? Property1 { get; set; }
    }
}
