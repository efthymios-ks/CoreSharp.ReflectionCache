using CoreSharp.ReflectionCache.Models;

namespace CoreSharp.ReflectionCache.Tests.Models;

public sealed class CachedFieldsTests
{
    [Fact]
    public void Constructor_WhenTypeIsNull_ShouldNotThrowException()
    {
        // Arrange
        Type? type = null;

        // Act
        void Action()
            => _ = new CachedFields(type);

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
        var cachedFields = new CachedFields(type);

        // Assert
        Assert.NotNull(cachedFields);
        Assert.Empty(cachedFields);
    }

    [Fact]
    public void Constructor_WhenCalled_ShouldInitializeFields()
    {
        // Arrange
        var type = typeof(DummyClass);

        // Act
        var cachedFields = new CachedFields(type);

        // Assert
        Assert.NotNull(cachedFields);
        Assert.Single(cachedFields);
        Assert.DoesNotContain(nameof(DummyClass.Property1), cachedFields.Keys);
        Assert.Contains(nameof(DummyClass.Field1), cachedFields.Keys);
    }

    private sealed class DummyClass
    {
        public string? Field1;
        public string? Property1 { get; set; }
    }
}
