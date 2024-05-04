using CoreSharp.ReflectionCache.Models;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Reflection;

namespace Tests.Models;

[TestFixture]
public sealed class CachedMethodTests
{
    [Test]
    public void Constructor_WhenMethodInfoIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        MethodInfo methodInfo = null;

        // Act
        Action action = () => _ = new CachedMethod(methodInfo);

        // Assert
        action.Should().ThrowExactly<ArgumentNullException>();
    }

    [Test]
    public void Constructor_WhenConstructorInfoIsNotNull_ShouldNotThrowException()
    {
        // Arrange
        var methodInfo = typeof(DummyClass)
            .GetMethod(nameof(DummyClass.Process));

        // Act
        Action action = () => _ = new CachedMethod(methodInfo);

        // Assert
        action.Should().NotThrow();
    }

    [Test]
    public void ReturnType_WhenCalled_ShouldReturnMethodInfoReturnType()
    {
        // Arrange
        var methodInfo = typeof(DummyClass)
            .GetMethod(nameof(DummyClass.Process));
        var cachedMethod = new CachedMethod(methodInfo);

        // Act
        var returnType = cachedMethod.ReturnType;

        // Assert
        returnType.Should().Be(typeof(string));
    }

    [Test]
    public void Parameters_WhenCalled_ShouldReturnParameterInfoArray()
    {
        // Arrange
        var methodInfo = typeof(DummyClass)
            .GetMethod(nameof(DummyClass.Process));
        var cachedMethod = new CachedMethod(methodInfo);

        // Act
        var parameters = cachedMethod.Parameters;

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
        var methodInfo = typeof(DummyClass)
            .GetMethod(nameof(DummyClass.Process));
        var cachedMethod = new CachedMethod(methodInfo);

        // Act
        var result = cachedMethod.Invoke<string>(parent: null, 1);

        // Assert 
        result.Should().Be("1");
    }

    [Test]
    public void Invoke_WhenCalled_ShouldInvokeConstructor()
    {
        // Arrange
        var methodInfo = typeof(DummyClass)
            .GetMethod(nameof(DummyClass.Process));
        var cachedMethod = new CachedMethod(methodInfo);

        // Act
        var result = cachedMethod.Invoke(parent: null, 1);

        // Assert
        var resultAsString = result.Should().BeOfType<string>().Subject;
        resultAsString.Should().Be("1");
    }

    private static class DummyClass
    {
        public static string Process(int value)
            => value.ToString();
    }
}