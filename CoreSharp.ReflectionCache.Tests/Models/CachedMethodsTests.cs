using CoreSharp.ReflectionCache.Models;

namespace CoreSharp.ReflectionCache.Tests.Models;

public sealed class CachedMethodsTests
{
    [Fact]
    public void Constructor_WhenTypeIsNull_ShouldNotThrowException()
    {
        // Arrange
        Type? type = null;

        // Act
        void Action()
            => _ = new CachedMethods(type);

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
        var cachedMethods = new CachedMethods(type);

        // Assert
        Assert.NotNull(cachedMethods);
        Assert.Empty(cachedMethods);
    }

    [Fact]
    public void Constructor_WhenCalled_ShouldInitializeFields()
    {
        // Arrange
        var type = typeof(DummyClass);

        // Act
        var cachedMethods = new CachedMethods(type);

        // Assert
        Assert.NotNull(cachedMethods);
        var processMethod = cachedMethods.FirstOrDefault(method => method.Name == nameof(DummyClass.Process));
        Assert.NotNull(processMethod);
        Assert.Equal(typeof(string), processMethod!.ReturnType);
        Assert.Equal(nameof(DummyClass.Process), processMethod.Name);

        var parameters = processMethod.Parameters;
        Assert.NotNull(parameters);
        Assert.Single(parameters);
        var parameter = parameters[0];
        Assert.Equal(typeof(int), parameter.ParameterType);
    }

    private static class DummyClass
    {
        public static string Process(int value)
            => value.ToString();
    }
}
