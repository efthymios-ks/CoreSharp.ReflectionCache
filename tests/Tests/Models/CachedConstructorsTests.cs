using CoreSharp.ReflectionCache.Models;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Linq;

namespace Tests.Models;

[TestFixture]
public sealed class CachedConstructorsTests
{
    [Test]
    public void Constructor_WhenTypeIsNull_ShouldNotThrowException()
    {
        // Arrange
        Type type = null!;

        // Act
        Action action = () => _ = new CachedFields(type);

        // Assert
        action.Should().NotThrow();
    }

    [Test]
    public void Constructor_WhenTypeIsNull_ShouldInitializeAsEmpty()
    {
        // Arrange
        Type type = null!;

        // Act
        var cachedConstructors = new CachedConstructors(type);

        // Assert
        cachedConstructors.Should().NotBeNull();
        cachedConstructors.Should().BeEmpty();
    }

    [Test]
    public void Constructor_WhenCalled_ShouldInitializeFields()
    {
        // Arrange
        var type = typeof(DummyClass);

        // Act
        var cachedConstuctors = new CachedConstructors(type);

        // Assert
        cachedConstuctors.Should().NotBeNull();
        cachedConstuctors.Should().HaveCount(1);
        var constructor = cachedConstuctors.First();
        var parameters = constructor.Parameters;
        parameters.Should().NotBeNull();
        parameters.Should().HaveCount(1);
        var parameter = parameters[0];
        parameter.ParameterType.Should().Be(typeof(int));

    }

    private sealed class DummyClass
    {
        public DummyClass(int _)
        {
        }
    }
}
