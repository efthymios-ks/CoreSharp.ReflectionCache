using FluentAssertions;
using NUnit.Framework;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Tests.Internal;

namespace CoreSharp.ReflectionCache.Models.Tests;

[TestFixture]
public sealed class CachedAttributesTests
{
    [Test]
    public void Constructor_WithMemberInfo_ShouldInitializeAttributes()
    {
        // Arrange
        var memberInfo = typeof(DummyClass).GetProperty(nameof(DummyClass.PropertyWith1Attribute));

        // Act
        var cachedAttributes = new CachedAttributes(memberInfo);

        // Assert
        cachedAttributes.Should().NotBeNull();
        cachedAttributes.Count.Should().BeGreaterThan(0);
    }

    [Test]
    public void Constructor_WithNullMemberInfo_ShouldInitializeEmptyAttributes()
    {
        // Arrange
        MemberInfo memberInfo = null;

        // Act
        var cachedAttributes = new CachedAttributes(memberInfo);

        // Assert
        cachedAttributes.Should().NotBeNull();
        cachedAttributes.Should().BeEmpty();
    }

    [Test]
    public void OfType_WhenAttributeExists_ShouldReturnAttribute()
    {
        // Arrange
        var propertyInfo = typeof(DummyClass).GetProperty(nameof(DummyClass.PropertyWith1Attribute));
        var cachedAttributes = new CachedAttributes(propertyInfo);

        // Act
        var displayAttribute = cachedAttributes.OfType<DisplayAttribute>();

        // Assert
        displayAttribute.Should().NotBeNull();
        displayAttribute.Name.Should().Be("Property1_Display");
    }

    [Test]
    public void OfType_WhenAttributeDoesNotExist_ShouldReturnNull()
    {
        // Arrange
        var propertyInfo = typeof(DummyClass).GetProperty(nameof(DummyClass.PropertyWith1Attribute));
        var cachedAttributes = new CachedAttributes(propertyInfo);

        // Act
        var nonExistentAttribute = cachedAttributes.OfType<DummyNonExistentAttribute>();

        // Assert
        nonExistentAttribute.Should().BeNull();
    }

    [Test]
    public void OfTypeAll_WhenAttributesExist_ShouldReturnAllAttributes()
    {
        // Arrange
        var propertyInfo = typeof(DummyClass).GetProperty(nameof(DummyClass.PropertyWith2Attributes));
        var cachedAttributes = new CachedAttributes(propertyInfo);

        // Act
        var displayAttributes = cachedAttributes.OfTypeAll<DisplayAttribute>();

        // Assert
        displayAttributes.Should().NotBeEmpty();
        displayAttributes.Should().HaveCount(1);
        displayAttributes[0].Name.Should().Be("Property2_Display");
    }

    [Test]
    public void OfTypeAll_WhenAttributesDoNotExist_ShouldReturnEmptyArray()
    {
        // Arrange
        var propertyInfo = typeof(DummyClass).GetProperty(nameof(DummyClass.PropertyWith2Attributes));
        var cachedAttributes = new CachedAttributes(propertyInfo);

        // Act
        var nonExistentAttributes = cachedAttributes.OfTypeAll<DummyNonExistentAttribute>();

        // Assert
        nonExistentAttributes.Should().BeEmpty();
    }
}
