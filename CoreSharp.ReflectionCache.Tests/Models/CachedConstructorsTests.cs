using CoreSharp.ReflectionCache.Models;
using FluentAssertions;
using NUnit.Framework;

namespace Tests.Models;

[TestFixture]
public sealed class CachedConstructorsTests
{
    [Test]
    public void Constructor_WhenTypeIsNull_ShouldNotThrowException()
    {
        // Arrange
        Type? type = null;

        // Act
        Action action = () => _ = new CachedConstructors(type);

        // Assert
        _ = action.Should().NotThrow();
    }

    [Test]
    public void Constructor_WhenTypeIsNull_ShouldInitializeAsEmpty()
    {
        // Arrange
        Type? type = null;

        // Act
        var cachedConstructors = new CachedConstructors(type);

        // Assert
        _ = cachedConstructors.Should().NotBeNull();
        _ = cachedConstructors.Should().BeEmpty();
    }

    [Test]
    public void Constructor_WhenCalled_ShouldInitializeFields()
    {
        // Arrange
        var type = typeof(DummyClass);

        // Act
        var cachedConstuctors = new CachedConstructors(type);

        // Assert
        _ = cachedConstuctors.Should().NotBeNull();
        _ = cachedConstuctors.Should().HaveCount(1);
        var constructor = cachedConstuctors.First();
        var parameters = constructor.Parameters;
        _ = parameters.Should().NotBeNull();
        _ = parameters.Should().HaveCount(1);
        var parameter = parameters[0];
        _ = parameter.ParameterType.Should().Be(typeof(int));

    }

    private sealed class DummyClass(int _)
    {
    }
}
