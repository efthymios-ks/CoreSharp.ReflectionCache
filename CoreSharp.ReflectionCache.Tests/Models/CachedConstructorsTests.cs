using CoreSharp.ReflectionCache.Models;

namespace CoreSharp.ReflectionCache.Tests.Models;

public sealed class CachedConstructorsTests
{
    [Fact]
    public void Constructor_WhenTypeIsNull_ShouldNotThrowException()
    {
        // Arrange
        Type? type = null;

        // Act
        void Action()
            => _ = new CachedConstructors(type);

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
        var cachedConstructors = new CachedConstructors(type);

        // Assert
        Assert.NotNull(cachedConstructors);
        Assert.Empty(cachedConstructors);
    }

    [Fact]
    public void Constructor_WhenCalled_ShouldInitializeFields()
    {
        // Arrange
        var type = typeof(DummyClass);

        // Act
        var cachedConstuctors = new CachedConstructors(type);

        // Assert
        Assert.NotNull(cachedConstuctors);
        Assert.Single(cachedConstuctors);
        var constructor = cachedConstuctors.First();
        var parameters = constructor.Parameters;
        Assert.NotNull(parameters);
        Assert.Single(parameters);
        var parameter = parameters[0];
        Assert.Equal(typeof(int), parameter.ParameterType);
    }

    private sealed class DummyClass(int _)
    {
    }
}
