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
        action.Should().NotThrow();
    }

    [Test]
    public void Constructor_WhenTypeIsNull_ShouldInitializeAsEmpty()
    {
        // Arrange
        Type? type = null;

        // Act
        var cachedMethods = new CachedMethods(type);

        // Assert
        cachedMethods.Should().NotBeNull();
        cachedMethods.Should().BeEmpty();
    }

    [Test]
    public void Constructor_WhenCalled_ShouldInitializeFields()
    {
        // Arrange
        var type = typeof(DummyClass);

        // Act
        var cachedMethods = new CachedMethods(type);

        // Assert
        cachedMethods.Should().NotBeNull();
        var processMethod = cachedMethods.FirstOrDefault(method => method.Name == nameof(DummyClass.Process));
        processMethod.Should().NotBeNull();
        processMethod!.ReturnType.Should().Be(typeof(string));
        processMethod.Name.Should().Be(nameof(DummyClass.Process));

        var parameters = processMethod.Parameters;
        parameters.Should().NotBeNull();
        parameters.Should().HaveCount(1);
        var parameter = parameters[0];
        parameter.ParameterType.Should().Be(typeof(int));
    }

    private static class DummyClass
    {
        public static string Process(int value)
            => value.ToString();
    }
}
