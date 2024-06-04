using CoreSharp.ReflectionCache.Models;
using FluentAssertions;
using NUnit.Framework;

namespace Tests.Models;

[TestFixture]
public sealed class CachedFieldsTests
{
    [Test]
    public void Constructor_WhenTypeIsNull_ShouldNotThrowException()
    {
        // Arrange
        Type? type = null;

        // Act
        Action action = () => _ = new CachedFields(type);

        // Assert
        _ = action.Should().NotThrow();
    }

    [Test]
    public void Constructor_WhenTypeIsNull_ShouldInitializeAsEmpty()
    {
        // Arrange
        Type? type = null;

        // Act
        var cachedFields = new CachedFields(type);

        // Assert
        _ = cachedFields.Should().NotBeNull();
        _ = cachedFields.Should().BeEmpty();
    }

    [Test]
    public void Constructor_WhenCalled_ShouldInitializeFields()
    {
        // Arrange
        var type = typeof(DummyClass);

        // Act
        var cachedFields = new CachedFields(type);

        // Assert
        _ = cachedFields.Should().NotBeNull();
        _ = cachedFields.Should().HaveCount(1);
        _ = cachedFields.Should().NotContainKey(nameof(DummyClass.Property1));
        _ = cachedFields.Should().ContainKey(nameof(DummyClass.Field1));
    }

    private sealed class DummyClass
    {
        public string? Field1;
        public string? Property1 { get; set; }
    }
}
