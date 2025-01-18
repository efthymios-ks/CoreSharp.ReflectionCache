using CoreSharp.ReflectionCache.Models;
using System.Reflection;

namespace CoreSharp.ReflectionCache.Tests.Models;

public sealed class CachedConstructorTests
{
    [Fact]
    public void Constructor_WhenConstructorInfoIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        ConstructorInfo? constructorInfo = null;

        // Act
        void Action()
            => _ = new CachedConstructor(constructorInfo);

        // Assert
        Assert.Throws<ArgumentNullException>(Action);
    }

    [Fact]
    public void Constructor_WhenConstructorInfoIsNotNull_ShouldNotThrowException()
    {
        // Arrange
        var constructorInfo = typeof(DummyClass)
            .GetConstructors()[0];

        // Act
        void Action()
            => _ = new CachedConstructor(constructorInfo);

        // Assert
        var exception = Record.Exception(Action);
        Assert.Null(exception);
    }

    [Fact]
    public void Parameters_WhenCalled_ShouldReturnParameterInfoArray()
    {
        // Arrange
        var constructorInfo = typeof(DummyClass)
            .GetConstructors()[0];
        var cachedConstructor = new CachedConstructor(constructorInfo);

        // Act
        var parameters = cachedConstructor.Parameters;

        // Assert
        Assert.NotNull(parameters);
        Assert.Equal(1, parameters.Length);
        var parameter = parameters[0];
        Assert.Equal(typeof(int), parameter.ParameterType);
    }

    [Fact]
    public void Invoke_Generic_WhenCalled_ShouldInvokeConstructor()
    {
        // Arrange
        var constructorInfo = typeof(DummyClass)
            .GetConstructors()[0];
        var cachedConstructor = new CachedConstructor(constructorInfo);

        // Act
        var dummyClass = cachedConstructor.Invoke<DummyClass>(1);

        // Assert
        Assert.NotNull(dummyClass);
        Assert.Equal(1, dummyClass.Value);
    }

    [Fact]
    public void Invoke_WhenCalled_ShouldInvokeConstructor()
    {
        // Arrange
        var constructorInfo = typeof(DummyClass)
            .GetConstructors()[0];
        var cachedConstructor = new CachedConstructor(constructorInfo);

        // Act
        var dummyClassAsObject = cachedConstructor.Invoke(1);

        // Assert
        Assert.NotNull(dummyClassAsObject);
        var dummyClass = Assert.IsType<DummyClass>(dummyClassAsObject);
        Assert.Equal(1, dummyClass.Value);
    }

    private sealed class DummyClass(int value)
    {
        public int Value { get; } = value;
    }
}
