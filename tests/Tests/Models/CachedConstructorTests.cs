using CoreSharp.ReflectionCache.Models;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Reflection;

namespace Tests.Models;

[TestFixture]
public sealed class CachedConstructorTests
{
    [Test]
    public void Constructor_WhenConstructorInfoIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        ConstructorInfo constructorInfo = null;

        // Act
        Action action = () => _ = new CachedConstructor(constructorInfo);

        // Assert
        action.Should().ThrowExactly<ArgumentNullException>();
    }

    [Test]
    public void Constructor_WhenConstructorInfoIsNotNull_ShouldNotThrowException()
    {
        // Arrange
        var constructorInfo = typeof(DummyClass)
            .GetConstructors()[0];

        // Act
        Action action = () => _ = new CachedConstructor(constructorInfo);

        // Assert
        action.Should().NotThrow();
    }

    [Test]
    public void Parameters_WhenCalled_ShouldReturnParameterInfoArray()
    {
        // Arrange
        var constructorInfo = typeof(DummyClass)
            .GetConstructors()[0];
        var cachedConstructor = new CachedConstructor(constructorInfo);

        // Act
        var parameters = cachedConstructor.Parameters;

        // Assert
        parameters.Should().NotBeNull();
        parameters.Should().HaveCount(1);
        var parameter = parameters[0];
        parameter.ParameterType.Should().Be(typeof(int));
    }

    [Test]
    public void Invoke_Generic_WhenCalled_ShouldInvokeConstructor()
    {
        // Arrange
        var constructorInfo = typeof(DummyClass)
            .GetConstructors()[0];
        var cachedConstructor = new CachedConstructor(constructorInfo);

        // Act
        var dummyClass = cachedConstructor.Invoke<DummyClass>(1);

        // Assert
        dummyClass.Should().NotBeNull();
        dummyClass.Value.Should().Be(1);
    }

    [Test]
    public void Invoke_WhenCalled_ShouldInvokeConstructor()
    {
        // Arrange
        var constructorInfo = typeof(DummyClass)
            .GetConstructors()[0];
        var cachedConstructor = new CachedConstructor(constructorInfo);

        // Act
        var dummyClassAsObject = cachedConstructor.Invoke(1);

        // Assert
        dummyClassAsObject.Should().NotBeNull();
        var dummyClass = dummyClassAsObject.Should().BeOfType<DummyClass>().Subject;
        dummyClass.Value.Should().Be(1);
    }

    private sealed class DummyClass
    {
        public DummyClass(int value)
            => Value = value;

        public int Value { get; }
    }
}
