using FluentAssertions;
using NUnit.Framework;
using System;
using System.Reflection;
using Tests.Internal;

namespace CoreSharp.ReflectionCache.Models.Tests;

[TestFixture]
public class CachedFieldTests
{
    [Test]
    public void Type_ShouldReturnFieldType()
    {
        // Arrange
        var fieldInfo = typeof(DummyClass).GetField(nameof(DummyClass.FieldWith1Attribute));
        var cachedField = new CachedField(fieldInfo);

        // Act
        var fieldType = cachedField.Type;

        // Assert
        fieldType.Should().NotBeNull();
        fieldType.Should().Be(new DummyClass().FieldWith1Attribute.GetType());
    }

    [Test]
    public void CanWrite_WithReadWrirwField_ShouldReturnTrue()
    {
        // Arrange
        var fieldInfo = typeof(DummyClass).GetField(nameof(DummyClass.FieldWith1Attribute));
        var cachedField = new CachedField(fieldInfo);

        // Act
        var canWrite = cachedField.CanWrite;

        // Assert
        canWrite.Should().BeTrue();
    }

    [Test]
    public void CanWrite_WithReadOnlyField_ShouldReturnFalse()
    {
        // Arrange
        var fieldInfo = typeof(DummyClass).GetField(nameof(DummyClass.ReadOnlyField1));
        var cachedField = new CachedField(fieldInfo);

        // Act
        var canWrite = cachedField.CanWrite;

        // Assert
        canWrite.Should().BeFalse();
    }

    [Test]
    public void CanWrite_WithConstField_ShouldReturnFalse()
    {
        // Arrange
        var fieldInfo = typeof(DummyClass).GetField(nameof(DummyClass.ConstField1));
        var cachedField = new CachedField(fieldInfo);

        // Act
        var canWrite = cachedField.CanWrite;

        // Assert
        canWrite.Should().BeFalse();
    }

    [Test]
    public void GetValue_WithValidValueType_ShouldReturnFieldValue()
    {
        // Arrange 
        const string value = "value";
        var parent = new DummyClass { FieldWith1Attribute = value };
        var fieldInfo = typeof(DummyClass).GetField(nameof(DummyClass.FieldWith1Attribute));
        var cachedField = new CachedField(fieldInfo);

        // Act
        var readValue = cachedField.GetValue<string>(parent);

        // Assert
        readValue.Should().NotBeNull();
        readValue.Should().Be(value);
    }

    [Test]
    public void GetValue_WithInvalidValueType_ShouldThrowInvalidCastException()
    {
        // Arrange  
        var parent = new DummyClass { FieldWith1Attribute = "value" };
        var fieldInfo = typeof(DummyClass).GetField(nameof(DummyClass.FieldWith1Attribute));
        var cachedField = new CachedField(fieldInfo);

        // Act
        Action action = () => cachedField.GetValue<int>(parent);

        // Assert 
        action.Should().ThrowExactly<InvalidCastException>();
    }

    [Test]
    public void GetValue_WithInvalidParent_ShouldThrowTargetException()
    {
        // Arrange 
        var parent = new DummyNonExistentAttribute();
        var fieldInfo = typeof(DummyClass).GetField(nameof(DummyClass.FieldWith1Attribute));
        var cachedField = new CachedField(fieldInfo);

        // Act
        Action action = () => cachedField.GetValue<string>(parent);

        // Assert
        action.Should().ThrowExactly<ArgumentException>();
    }

    [Test]
    public void GetValue_WithNullParent_ShouldThrowTargetException()
    {
        // Arrange 
        var fieldInfo = typeof(DummyClass).GetField(nameof(DummyClass.FieldWith1Attribute));
        var cachedField = new CachedField(fieldInfo);

        // Act
        Action action = () => cachedField.GetValue<string>(null);

        // Assert
        action.Should().ThrowExactly<TargetException>();
    }

    [Test]
    public void SetValue_WithValidValueType_ShouldSetFieldValue()
    {
        // Arrange 
        const string value = "value";
        var parent = new DummyClass();
        var fieldInfo = typeof(DummyClass).GetField(nameof(DummyClass.FieldWith1Attribute));
        var cachedField = new CachedField(fieldInfo);

        // Act
        cachedField.SetValue(parent, value);

        // Assert
        parent.FieldWith1Attribute.Should().NotBeNull();
        parent.FieldWith1Attribute.Should().Be(value);
    }

    [Test]
    public void SetValue_WithInvalidValueType_ShouldThrowArgumentException()
    {
        // Arrange  
        var parent = new DummyClass();
        var fieldInfo = typeof(DummyClass).GetField(nameof(DummyClass.FieldWith1Attribute));
        var cachedField = new CachedField(fieldInfo);

        // Act
        Action action = () => cachedField.SetValue(parent, 0);

        // Assert
        action.Should().ThrowExactly<ArgumentException>();
    }

    [Test]
    public void SetValue_WithInvalidParent_ShouldThrowTargetException()
    {
        // Arrange 
        const string value = "value";
        var parent = new DummyNonExistentAttribute();
        var fieldInfo = typeof(DummyClass).GetField(nameof(DummyClass.FieldWith1Attribute));
        var cachedField = new CachedField(fieldInfo);

        // Act
        Action action = () => cachedField.SetValue(parent, value);

        // Assert
        action.Should().ThrowExactly<ArgumentException>();
    }

    [Test]
    public void SetValue_WithNullParent_ShouldThrowTargetException()
    {
        // Arrange 
        const string value = "value";
        DummyClass parent = null;
        var fieldInfo = typeof(DummyClass).GetField(nameof(DummyClass.FieldWith1Attribute));
        var cachedField = new CachedField(fieldInfo);

        // Act
        Action action = () => cachedField.SetValue(parent, value);

        // Assert
        action.Should().ThrowExactly<TargetException>();
    }
}