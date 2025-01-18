using CoreSharp.ReflectionCache.Models;
using System.Reflection;

namespace CoreSharp.ReflectionCache.Tests.Models;

public sealed class CachedMethodTests
{
    [Fact]
    public void Constructor_WhenMethodInfoIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        MethodInfo? methodInfo = null;

        // Act
        void Action()
            => _ = new CachedMethod(methodInfo);

        // Assert
        Assert.Throws<ArgumentNullException>(Action);
    }

    [Fact]
    public void Constructor_WhenConstructorInfoIsNotNull_ShouldNotThrowException()
    {
        // Arrange
        var methodInfo = typeof(DummyClass)
            .GetMethod(nameof(DummyClass.Process));

        // Act
        void Action()
            => _ = new CachedMethod(methodInfo);

        // Assert
        var exception = Record.Exception(Action);
        Assert.Null(exception);
    }

    [Fact]
    public void ReturnType_WhenCalled_ShouldReturnMethodInfoReturnType()
    {
        // Arrange
        var methodInfo = typeof(DummyClass)
            .GetMethod(nameof(DummyClass.Process));
        var cachedMethod = new CachedMethod(methodInfo);

        // Act
        var returnType = cachedMethod.ReturnType;

        // Assert
        Assert.Equal(typeof(string), returnType);
    }

    [Fact]
    public void Parameters_WhenCalled_ShouldReturnParameterInfoArray()
    {
        // Arrange
        var methodInfo = typeof(DummyClass)
            .GetMethod(nameof(DummyClass.Process));
        var cachedMethod = new CachedMethod(methodInfo);

        // Act
        var parameters = cachedMethod.Parameters;

        // Assert
        Assert.NotNull(parameters);
        Assert.Single(parameters);
        var parameter = parameters[0];
        Assert.Equal(typeof(int), parameter.ParameterType);
    }

    [Fact]
    public void Invoke_Generic_WhenCalled_ShouldInvokeConstructor()
    {
        // Arrange
        var methodInfo = typeof(DummyClass)
            .GetMethod(nameof(DummyClass.Process));
        var cachedMethod = new CachedMethod(methodInfo);

        // Act
        var result = cachedMethod.Invoke<string>(parent: null, 1);

        // Assert 
        Assert.Equal("1", result);
    }

    [Fact]
    public void Invoke_WhenCalled_ShouldInvokeConstructor()
    {
        // Arrange
        var methodInfo = typeof(DummyClass)
            .GetMethod(nameof(DummyClass.Process));
        var cachedMethod = new CachedMethod(methodInfo);

        // Act
        var result = cachedMethod.Invoke(parent: null, 1);

        // Assert
        var resultAsString = Assert.IsType<string>(result);
        Assert.Equal("1", resultAsString);
    }

    private static class DummyClass
    {
        public static string Process(int value)
            => value.ToString();
    }
}
