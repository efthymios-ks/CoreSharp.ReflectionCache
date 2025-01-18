using CoreSharp.ReflectionCache.Models;
using System.Reflection;

namespace CoreSharp.ReflectionCache.Tests.Models;

public sealed class CachedPropertyTests
{
    [Fact]
    public void Constructor_WhenPropertyInfoIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        PropertyInfo? propertyInfo = null;

        // Act
        void Action()
            => _ = new CachedProperty(propertyInfo);

        // Assert
        Assert.Throws<ArgumentNullException>(Action);
    }

    [Fact]
    public void Constructor_WhenPropertyInfoIsNotNull_ShouldNotThrowException()
    {
        // Arrange
        var propertyInfo = typeof(DummyClass)
            .GetProperty(nameof(DummyClass.Property));

        // Act
        void Action()
            => _ = new CachedProperty(propertyInfo);

        // Assert
        var exception = Record.Exception(Action);
        Assert.Null(exception);
    }

    [Fact]
    public void Type_WhenCalled_ShouldReturnFieldType()
    {
        // Arrange
        var propertyInfo = typeof(DummyClass)
            .GetProperty(nameof(DummyClass.Property));

        var cachedProperty = new CachedProperty(propertyInfo);

        // Act
        var propertyType = cachedProperty.Type;

        // Assert
        Assert.NotNull(propertyType);
        Assert.Equal(typeof(string), propertyType);
    }

    [Fact]
    public void CanWrite_WhenPropertyIsReadOnly_ShouldReturnFalse()
    {
        // Arrange
        var propertyInfo = typeof(DummyClass)
            .GetProperty(nameof(DummyClass.ReadOnlyProperty));
        var cachedProperty = new CachedProperty(propertyInfo);

        // Act
        var canWrite = cachedProperty.CanWrite;

        // Assert
        Assert.False(canWrite);
    }

    [Fact]
    public void CanWrite_WhenFieldIsWritable_ShouldReturnTrue()
    {
        // Arrange
        var propertyInfo = typeof(DummyClass)
            .GetProperty(nameof(DummyClass.Property));
        var cachedProperty = new CachedProperty(propertyInfo);

        // Act
        var canWrite = cachedProperty.CanWrite;

        // Assert
        Assert.True(canWrite);
    }

    [Fact]
    public void CanRead_WhenPropertyIsWriteOnly_ShouldReturnFalse()
    {
        // Arrange
        var propertyInfo = typeof(DummyClass)
            .GetProperty(nameof(DummyClass.WriteOnlyProperty));
        var cachedProperty = new CachedProperty(propertyInfo);

        // Act
        var canRead = cachedProperty.CanRead;

        // Assert
        Assert.False(canRead);
    }

    [Fact]
    public void CanRead_WhenFieldIsReadable_ShouldReturnTrue()
    {
        // Arrange
        var propertyInfo = typeof(DummyClass)
            .GetProperty(nameof(DummyClass.Property));
        var cachedProperty = new CachedProperty(propertyInfo);

        // Act
        var canRead = cachedProperty.CanRead;

        // Assert
        Assert.True(canRead);
    }

    [Fact]
    public void GetValue_WhenCalled_ShouldReturnFieldValue()
    {
        // Arrange 
        var parent = new DummyClass()
        {
            Property = "Value"
        };
        var propertyInfo = typeof(DummyClass)
            .GetProperty(nameof(DummyClass.Property));
        var cachedProperty = new CachedProperty(propertyInfo);

        // Act
        var valueRead = cachedProperty.GetValue(parent);

        // Assert
        Assert.NotNull(valueRead);
        Assert.Equal(parent.Property, valueRead);
    }

    [Fact]
    public void GetValue_WhenGetterIsNotPublic_ShouldReturnPrivateGetter()
    {
        // Arrange
        var propertyInfo = Substitute.For<PropertyInfo>();
        var cachedProperty = new CachedProperty(propertyInfo);

        var getterMethodInfoMock = Substitute.For<MethodInfo>();
        propertyInfo
            .GetGetMethod(true)
            .Returns(getterMethodInfoMock);

        // Act 
        void Action()
            => _ = cachedProperty.GetValue(new object());

        // Assert 
        var exception = Assert.Throws<ArgumentException>(Action);
        Assert.Equal("MethodInfo must be a runtime MethodInfo object. (Parameter 'method')", exception.Message);

        propertyInfo
          .GetGetMethod(true)
          .Received(1);
    }

    [Fact]
    public void GetValue_WhenNoGetterIsFound_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var propertyInfo = Substitute.For<PropertyInfo>();
        var cachedProperty = new CachedProperty(propertyInfo);

        var parent = new DummyClass();
        propertyInfo
            .DeclaringType
            .Returns(parent.GetType());

        propertyInfo
            .Name
            .Returns(nameof(DummyClass.Property));

        // Act 
        void Action()
            => _ = cachedProperty.GetValue(parent);

        // Assert 
        Assert.Throws<InvalidOperationException>(Action);
    }

    [Fact]
    public void SetValue_WhenCalled_ShouldSetValue()
    {
        // Arrange 
        const string valueToSet = "Value";
        var parent = new DummyClass();
        var propertyInfo = typeof(DummyClass)
            .GetProperty(nameof(DummyClass.Property));
        var cacheProperty = new CachedProperty(propertyInfo);

        // Act
        cacheProperty.SetValue(parent, valueToSet);

        // Assert 
        Assert.Equal(valueToSet, parent.Property);
    }

    [Fact]
    public void SetValue_WhenGetterIsNotPublic_ShouldReturnPrivateGetter()
    {
        // Arrange
        var propertyInfo = Substitute.For<PropertyInfo>();
        var cachedProperty = new CachedProperty(propertyInfo);

        var getterMethodInfoMock = Substitute.For<MethodInfo>();
        propertyInfo
            .GetSetMethod(true)
            .Returns(getterMethodInfoMock);

        // Act 
        void Action()
            => cachedProperty.SetValue(new object(), "");

        // Assert 
        var exception = Assert.Throws<ArgumentException>(Action);
        Assert.Equal("MethodInfo must be a runtime MethodInfo object. (Parameter 'method')", exception.Message);

        propertyInfo
          .GetSetMethod(true)
          .Received(1);
    }

    [Fact]
    public void SetValue_WhenNoGetterIsFound_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var propertyInfo = Substitute.For<PropertyInfo>();
        var cachedProperty = new CachedProperty(propertyInfo);

        var parent = new DummyClass();
        propertyInfo
            .DeclaringType
            .Returns(parent.GetType());

        propertyInfo
            .Name
            .Returns(nameof(DummyClass.Property));

        // Act 
        void Action()
            => cachedProperty.SetValue(parent, "");

        // Assert 
        Assert.Throws<InvalidOperationException>(Action);
    }

    private sealed class DummyClass
    {
        private static string? _writeOnlyProperty;

        public string Property { get; set; } = "Property";

        public static string ReadOnlyProperty
            => "ReadOnlyProperty";

        public static string WriteOnlyProperty
        {
            set => _writeOnlyProperty = value;
        }
    }
}
