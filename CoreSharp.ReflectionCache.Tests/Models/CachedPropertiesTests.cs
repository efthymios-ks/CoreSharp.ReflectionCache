using CoreSharp.ReflectionCache.Models;

namespace CoreSharp.ReflectionCache.Tests.Models;

public sealed class CachedPropertiesTests
{
    [Fact]
    public void Constructor_WhenTypeIsNull_ShouldNotThrowException()
    {
        // Arrange
        Type? type = null;

        // Act
        void Action()
            => _ = new CachedProperties(type);

        // Assert
        var exception = Record.Exception(Action);
        Assert.Null(exception);
    }

    [Fact]
    public void Constructor_WhenTypeIsNull_ShouldInitializeAsEmpty()
    {
        // Arrange
        Type? type = null;

        // Act
        var cachedProperties = new CachedProperties(type);

        // Assert
        Assert.NotNull(cachedProperties);
        Assert.Empty(cachedProperties);
    }

    [Fact]
    public void Constructor_WhenCalled_ShouldInitializeFields()
    {
        // Arrange
        var type = typeof(DummyClass);

        // Act
        var cachedProperties = new CachedProperties(type);

        // Assert
        Assert.NotNull(cachedProperties);
        Assert.Single(cachedProperties);
        Assert.DoesNotContain(nameof(DummyClass.Field1), cachedProperties.Keys);
        Assert.Contains(nameof(DummyClass.Property1), cachedProperties.Keys);
    }

    private sealed class DummyClass
    {
        public string? Field1;
        public string? Property1 { get; set; }
    }
}
