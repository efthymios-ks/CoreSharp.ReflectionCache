using CoreSharp.ReflectionCache.Models;
using FluentAssertions;
using NUnit.Framework;
using System.Reflection;

namespace Tests.Models;

[TestFixture]
public sealed class CachedFieldTests
{
    [Test]
    public void Constructor_WhenFieldInfoIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        FieldInfo? fieldInfo = null;

        // Act
        Action action = () => _ = new CachedField(fieldInfo);

        // Assert
        _ = action.Should().ThrowExactly<ArgumentNullException>();
    }

    [Test]
    public void Constructor_WhenFieldInfoIsNotNull_ShouldNotThrowException()
    {
        // Arrange
        var fieldInfo = typeof(DummyClass)
            .GetField(nameof(DummyClass.Field));

        // Act
        Action action = () => _ = new CachedField(fieldInfo);

        // Assert
        _ = action.Should().NotThrow();
    }

    [Test]
    public void Type_WhenCalled_ShouldReturnFieldType()
    {
        // Arrange
        var fieldInfo = typeof(DummyClass)
            .GetField(nameof(DummyClass.Field));

        var cachedField = new CachedField(fieldInfo);

        // Act
        var fieldType = cachedField.Type;

        // Assert
        _ = fieldType.Should().NotBeNull();
        _ = fieldType.Should().Be(typeof(string));
    }

    [Test]
    public void CanWrite_WhenFieldIsConst_ShouldReturnFalse()
    {
        // Arrange
        var fieldInfo = typeof(DummyClass)
            .GetField(nameof(DummyClass.ConstField));
        var cachedField = new CachedField(fieldInfo);

        // Act
        var canWrite = cachedField.CanWrite;

        // Assert
        _ = canWrite.Should().BeFalse();
    }

    [Test]
    public void CanWrite_WhenFieldIsReadOnly_ShouldReturnFalse()
    {
        // Arrange
        var fieldInfo = typeof(DummyClass)
            .GetField(nameof(DummyClass.ReadOnlyField));
        var cachedField = new CachedField(fieldInfo);

        // Act
        var canWrite = cachedField.CanWrite;

        // Assert
        _ = canWrite.Should().BeFalse();
    }

    [Test]
    public void CanWrite_WhenFieldIsReadWrite_ShouldReturnTrue()
    {
        // Arrange
        var fieldInfo = typeof(DummyClass)
            .GetField(nameof(DummyClass.Field));
        var cachedField = new CachedField(fieldInfo);

        // Act
        var canWrite = cachedField.CanWrite;

        // Assert
        _ = canWrite.Should().BeTrue();
    }

    [Test]
    public void GetValue_Generic_WhenCalled_ShouldReturnFieldValue()
    {
        // Arrange 
        var parent = new DummyClass()
        {
            Field = "Value"
        };
        var fieldInfo = typeof(DummyClass)
            .GetField(nameof(DummyClass.Field));
        var cachedField = new CachedField(fieldInfo);

        // Act
        var valueRead = cachedField.GetValue<string>(parent);

        // Assert
        _ = valueRead.Should().NotBeNull();
        _ = valueRead.Should().Be(parent.Field);
    }

    [Test]
    public void GetValue_WhenCalled_ShouldReturnFieldValue()
    {
        // Arrange 
        var parent = new DummyClass()
        {
            Field = "Value"
        };
        var fieldInfo = typeof(DummyClass)
            .GetField(nameof(DummyClass.Field));
        var cachedField = new CachedField(fieldInfo);

        // Act
        var valueRead = cachedField.GetValue(parent);

        // Assert
        _ = valueRead.Should().NotBeNull();
        var valueReadAsString = valueRead.Should().BeOfType<string>().Subject;
        _ = valueReadAsString.Should().Be(parent.Field);
    }

    [Test]
    public void SetValue_WhenCalled_ShouldSetValue()
    {
        // Arrange 
        const string valueToSet = "Value";
        var parent = new DummyClass();
        var fieldInfo = typeof(DummyClass)
            .GetField(nameof(DummyClass.Field));
        var cachedField = new CachedField(fieldInfo);

        // Act
        cachedField.SetValue(parent, valueToSet);

        // Assert 
        _ = parent.Field.Should().Be(valueToSet);
    }

    private sealed class DummyClass
    {
        public string? Field;
        public const string ConstField = "ConstField";
        public readonly string ReadOnlyField = "ReadOnlyField";
    }
}
