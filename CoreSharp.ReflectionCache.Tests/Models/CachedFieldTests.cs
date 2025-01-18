using CoreSharp.ReflectionCache.Models;
using System.Reflection;

namespace CoreSharp.ReflectionCache.Tests.Models;

public sealed class CachedFieldTests
{
    [Fact]
    public void Constructor_WhenFieldInfoIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        FieldInfo? fieldInfo = null;

        // Act
        void Action()
            => _ = new CachedField(fieldInfo);

        // Assert
        Assert.Throws<ArgumentNullException>(Action);
    }

    [Fact]
    public void Constructor_WhenFieldInfoIsNotNull_ShouldNotThrowException()
    {
        // Arrange
        var fieldInfo = typeof(DummyClass)
            .GetField(nameof(DummyClass.Field));

        // Act
        void Action()
            => _ = new CachedField(fieldInfo);

        // Assert
        var exception = Record.Exception(Action);
        Assert.Null(exception);
    }

    [Fact]
    public void Type_WhenCalled_ShouldReturnFieldType()
    {
        // Arrange
        var fieldInfo = typeof(DummyClass)
            .GetField(nameof(DummyClass.Field));

        var cachedField = new CachedField(fieldInfo);

        // Act
        var fieldType = cachedField.Type;

        // Assert
        Assert.NotNull(fieldType);
        Assert.Equal(typeof(string), fieldType);
    }

    [Fact]
    public void CanWrite_WhenFieldIsConst_ShouldReturnFalse()
    {
        // Arrange
        var fieldInfo = typeof(DummyClass)
            .GetField(nameof(DummyClass.ConstField));
        var cachedField = new CachedField(fieldInfo);

        // Act
        var canWrite = cachedField.CanWrite;

        // Assert
        Assert.False(canWrite);
    }

    [Fact]
    public void CanWrite_WhenFieldIsReadOnly_ShouldReturnFalse()
    {
        // Arrange
        var fieldInfo = typeof(DummyClass)
            .GetField(nameof(DummyClass.ReadOnlyField));
        var cachedField = new CachedField(fieldInfo);

        // Act
        var canWrite = cachedField.CanWrite;

        // Assert
        Assert.False(canWrite);
    }

    [Fact]
    public void CanWrite_WhenFieldIsReadWrite_ShouldReturnTrue()
    {
        // Arrange
        var fieldInfo = typeof(DummyClass)
            .GetField(nameof(DummyClass.Field));
        var cachedField = new CachedField(fieldInfo);

        // Act
        var canWrite = cachedField.CanWrite;

        // Assert
        Assert.True(canWrite);
    }

    [Fact]
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
        Assert.NotNull(valueRead);
        Assert.Equal(parent.Field, valueRead);
    }

    [Fact]
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
        Assert.NotNull(valueRead);
        var valueReadAsString = Assert.IsType<string>(valueRead);
        Assert.Equal(parent.Field, valueReadAsString);
    }

    [Fact]
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
        Assert.Equal(valueToSet, parent.Field);
    }

    private sealed class DummyClass
    {
        public string? Field;
        public const string ConstField = "ConstField";
        public readonly string ReadOnlyField = "ReadOnlyField";
    }
}
