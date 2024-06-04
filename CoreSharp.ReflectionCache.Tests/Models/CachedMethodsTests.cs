using CoreSharp.ReflectionCache.Models;
using FluentAssertions;
using NUnit.Framework;

namespace Tests.Models;

[TestFixture]
public sealed class CachedMethodsTests
{
    [Test]
    public void Constructor_WhenTypeIsNull_ShouldNotThrowException()
    {
        // Arrange
        Type? type = null;

        // Act
        Action action = () => _ = new CachedMethods(type);

        // Assert
        _ = action.Should().NotThrow();
    }

    [Test]
    public void Constructor_WhenTypeIsNull_ShouldInitializeAsEmpty()
    {
        // Arrange
        Type? type = null;

        // Act
        var cachedMethods = new CachedMethods(type);

        // Assert
        _ = cachedMethods.Should().NotBeNull();
        _ = cachedMethods.Should().BeEmpty();
    }

    [Test]
    public void Constructor_WhenCalled_ShouldInitializeFields()
    {
        // Arrange
        var type = typeof(DummyClass);

        // Act
        var cachedMethods = new CachedMethods(type);

        // Assert
        _ = cachedMethods.Should().NotBeNull();
        var processMethod = cachedMethods.FirstOrDefault(method => method.Name == nameof(DummyClass.Process));
        _ = processMethod.Should().NotBeNull();
        _ = processMethod!.ReturnType.Should().Be(typeof(string));
        _ = processMethod.Name.Should().Be(nameof(DummyClass.Process));

        var parameters = processMethod.Parameters;
        _ = parameters.Should().NotBeNull();
        _ = parameters.Should().HaveCount(1);
        var parameter = parameters[0];
        _ = parameter.ParameterType.Should().Be(typeof(int));
    }

    private static class DummyClass
    {
        public static string Process(int value)
            => value.ToString();
    }
}
