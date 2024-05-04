using CoreSharp.ReflectionCache.Models;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace Tests.Models;

[TestFixture]
public sealed class CachedAttributesTests
{
    [Test]
    public void Constructor_WhenMemberInfoHasValue_ShouldInitializeAttributes()
    {
        // Arrange
        var memberInfo = typeof(DummyClass)
            .GetProperty(nameof(DummyClass.PropertyWithAttributes));

        // Act
        var cachedAttributes = new CachedAttributes(memberInfo);

        // Assert
        cachedAttributes.Should().NotBeNull();
        cachedAttributes.Should().HaveCount(1);
        var displayAttribute = cachedAttributes.First().Should().BeOfType<DisplayAttribute>().Subject;
        displayAttribute.Name.Should().Be("PropertyWithAttributes_Display");
    }

    [Test]
    public void Constructor_WhenMemberInfoIsNull_ShouldInitializeEmptyAttributes()
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
    public void Constructor_WhenAttributesHasValue_ShouldInitializeAttributes()
    {
        // Arrange
        var displayAttribute = new DisplayAttribute { Name = "PropertyWithAttributes_Display" };
        var attributes = new Attribute[] { displayAttribute };

        // Act
        var cachedAttributes = new CachedAttributes(attributes);

        // Assert
        cachedAttributes.Should().NotBeNull();
        cachedAttributes.Should().HaveCount(1);
        var attribute = cachedAttributes.First();
        attribute.Should().Be(displayAttribute);
    }

    [Test]
    public void Constructor_WhenAttributesIsNull_ShouldInitializeEmptyAttributes()
    {
        // Arrange
        Attribute[] attributes = null;

        // Act
        var cachedAttributes = new CachedAttributes(attributes);

        // Assert
        cachedAttributes.Should().NotBeNull();
        cachedAttributes.Should().BeEmpty();
    }

    [Test]
    public void OfTypeGeneric_WhenAttributeExists_ShouldReturnAttribute()
    {
        // Arrange
        var propertyInfo = typeof(DummyClass)
            .GetProperty(nameof(DummyClass.PropertyWithAttributes));
        var cachedAttributes = new CachedAttributes(propertyInfo);

        // Act
        var displayAttribute = cachedAttributes.OfType<DisplayAttribute>();

        // Assert
        displayAttribute.Should().NotBeNull();
        displayAttribute.Name.Should().Be("PropertyWithAttributes_Display");
    }

    [Test]
    public void OfTypeGeneric_WhenAttributeDoesNotExist_ShouldReturnNull()
    {
        // Arrange
        var propertyInfo = typeof(DummyClass)
            .GetProperty(nameof(DummyClass.PropertyWithAttributes));
        var cachedAttributes = new CachedAttributes(propertyInfo);

        // Act
        var debuggerDisplayAttribute = cachedAttributes.OfType<DescriptionAttribute>();

        // Assert
        debuggerDisplayAttribute.Should().BeNull();
    }

    [Test]
    public void OfType_WhenAttributeExists_ShouldReturnAttribute()
    {
        // Arrange
        var propertyInfo = typeof(DummyClass)
            .GetProperty(nameof(DummyClass.PropertyWithAttributes));
        var cachedAttributes = new CachedAttributes(propertyInfo);

        // Act
        var attribute = cachedAttributes.OfType(typeof(DisplayAttribute));

        // Assert
        attribute.Should().NotBeNull();
        var displayAttribute = attribute.Should().BeOfType<DisplayAttribute>().Subject;
        displayAttribute.Name.Should().Be("PropertyWithAttributes_Display");
    }

    [Test]
    public void OfType_WhenAttributeDoesNotExist_ShouldReturnNull()
    {
        // Arrange
        var propertyInfo = typeof(DummyClass)
            .GetProperty(nameof(DummyClass.PropertyWithAttributes));
        var cachedAttributes = new CachedAttributes(propertyInfo);

        // Act
        var debuggerDisplayAttribute = cachedAttributes.OfType(typeof(DescriptionAttribute));

        // Assert
        debuggerDisplayAttribute.Should().BeNull();
    }

    [Test]
    public void OfTypeAllGeneric_WhenAttributesExist_ShouldReturnAllAttributes()
    {
        // Arrange
        var propertyInfo = typeof(DummyClass)
            .GetProperty(nameof(DummyClass.PropertyWithAttributes));
        var cachedAttributes = new CachedAttributes(propertyInfo);

        // Act
        var displayAttributes = cachedAttributes.OfTypeAll<DisplayAttribute>();

        // Assert
        displayAttributes.Should().NotBeEmpty();
        displayAttributes.Should().HaveCount(1);
        displayAttributes[0].Name.Should().Be("PropertyWithAttributes_Display");
    }

    [Test]
    public void OfTypeAllGeneric_WhenAttributesDoNotExist_ShouldReturnEmptyArray()
    {
        // Arrange
        var propertyInfo = typeof(DummyClass)
            .GetProperty(nameof(DummyClass.PropertyWithAttributes));
        var cachedAttributes = new CachedAttributes(propertyInfo);

        // Act
        var nonExistentAttributes = cachedAttributes.OfTypeAll<DescriptionAttribute>();

        // Assert
        nonExistentAttributes.Should().BeEmpty();
    }

    [Test]
    public void OfTypeAll_WhenAttributesExist_ShouldReturnAllAttributes()
    {
        // Arrange
        var propertyInfo = typeof(DummyClass)
            .GetProperty(nameof(DummyClass.PropertyWithAttributes));
        var cachedAttributes = new CachedAttributes(propertyInfo);

        // Act
        var displayAttributes = cachedAttributes.OfTypeAll(typeof(DisplayAttribute));

        // Assert
        displayAttributes.Should().NotBeEmpty();
        displayAttributes.Should().HaveCount(1);
        var displayAttribute = displayAttributes[0].Should().BeOfType<DisplayAttribute>().Subject;
        displayAttribute.Name.Should().Be("PropertyWithAttributes_Display");
    }

    [Test]
    public void OfTypeAll_WhenAttributesDoNotExist_ShouldReturnEmptyArray()
    {
        // Arrange
        var propertyInfo = typeof(DummyClass)
            .GetProperty(nameof(DummyClass.PropertyWithAttributes));
        var cachedAttributes = new CachedAttributes(propertyInfo);

        // Act
        var nonExistentAttributes = cachedAttributes.OfTypeAll(typeof(DescriptionAttribute));

        // Assert
        nonExistentAttributes.Should().BeEmpty();
    }

    private sealed class DummyClass
    {
        [Display(Name = "PropertyWithAttributes_Display")]
        public string PropertyWithAttributes { get; set; }
    }
}
