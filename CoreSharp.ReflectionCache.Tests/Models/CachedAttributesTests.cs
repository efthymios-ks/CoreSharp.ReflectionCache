using CoreSharp.ReflectionCache.Models;
using FluentAssertions;
using NUnit.Framework;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Tests.Models;

[TestFixture]
public sealed class CachedAttributesTests
{
    [Test]
    public void Constructor_WhenMemberInfoIsNotNull_ShouldInitializeAttributes()
    {
        // Arrange
        var memberInfo = typeof(DummyClass)
            .GetProperty(nameof(DummyClass.PropertyWithAttributes));

        // Act
        var cachedAttributes = new CachedAttributes(memberInfo);

        // Assert
        _ = cachedAttributes.Should().NotBeNull();
        _ = cachedAttributes.Should().HaveCount(1);
        var displayAttribute = cachedAttributes.First().Should().BeOfType<DisplayAttribute>().Subject;
        _ = displayAttribute.Name.Should().Be("PropertyWithAttributes_Display");
    }

    [Test]
    public void Constructor_WhenMemberInfoIsNull_ShouldInitializeEmptyAttributes()
    {
        // Arrange
        MemberInfo? memberInfo = null;

        // Act
        var cachedAttributes = new CachedAttributes(memberInfo);

        // Assert
        _ = cachedAttributes.Should().NotBeNull();
        _ = cachedAttributes.Should().BeEmpty();
    }

    [Test]
    public void Constructor_WhenAttributesIsNotNull_ShouldInitializeAttributes()
    {
        // Arrange
        var displayAttribute = new DisplayAttribute { Name = "PropertyWithAttributes_Display" };
        var attributes = new Attribute[] { displayAttribute };

        // Act
        var cachedAttributes = new CachedAttributes(attributes);

        // Assert
        _ = cachedAttributes.Should().NotBeNull();
        _ = cachedAttributes.Should().HaveCount(1);
        var attribute = cachedAttributes.First();
        _ = attribute.Should().Be(displayAttribute);
    }

    [Test]
    public void Constructor_WhenAttributesIsNull_ShouldInitializeEmptyAttributes()
    {
        // Arrange
        Attribute[]? attributes = null;

        // Act
        var cachedAttributes = new CachedAttributes(attributes);

        // Assert
        _ = cachedAttributes.Should().NotBeNull();
        _ = cachedAttributes.Should().BeEmpty();
    }

    [Test]
    public void OfType_Generic_WhenAttributeExists_ShouldReturnAttribute()
    {
        // Arrange
        var propertyInfo = typeof(DummyClass)
            .GetProperty(nameof(DummyClass.PropertyWithAttributes));
        var cachedAttributes = new CachedAttributes(propertyInfo);

        // Act
        var displayAttribute = cachedAttributes.OfType<DisplayAttribute>();

        // Assert
        _ = displayAttribute.Should().NotBeNull();
        _ = displayAttribute!.Name.Should().Be("PropertyWithAttributes_Display");
    }

    [Test]
    public void OfType_Generic_WhenAttributeDoesNotExist_ShouldReturnNull()
    {
        // Arrange
        var propertyInfo = typeof(DummyClass)
            .GetProperty(nameof(DummyClass.PropertyWithAttributes));
        var cachedAttributes = new CachedAttributes(propertyInfo);

        // Act
        var debuggerDisplayAttribute = cachedAttributes.OfType<DescriptionAttribute>();

        // Assert
        _ = debuggerDisplayAttribute.Should().BeNull();
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
        _ = attribute.Should().NotBeNull();
        var displayAttribute = attribute.Should().BeOfType<DisplayAttribute>().Subject;
        _ = displayAttribute.Name.Should().Be("PropertyWithAttributes_Display");
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
        _ = debuggerDisplayAttribute.Should().BeNull();
    }

    [Test]
    public void OfTypeAll_Generic_WhenAttributesExist_ShouldReturnAllAttributes()
    {
        // Arrange
        var propertyInfo = typeof(DummyClass)
            .GetProperty(nameof(DummyClass.PropertyWithAttributes));
        var cachedAttributes = new CachedAttributes(propertyInfo);

        // Act
        var displayAttributes = cachedAttributes.OfTypeAll<DisplayAttribute>();

        // Assert
        _ = displayAttributes.Should().NotBeEmpty();
        _ = displayAttributes.Should().HaveCount(1);
        _ = displayAttributes[0].Name.Should().Be("PropertyWithAttributes_Display");
    }

    [Test]
    public void OfTypeAll_Generic_WhenAttributesDoNotExist_ShouldReturnEmptyArray()
    {
        // Arrange
        var propertyInfo = typeof(DummyClass)
            .GetProperty(nameof(DummyClass.PropertyWithAttributes));
        var cachedAttributes = new CachedAttributes(propertyInfo);

        // Act
        var nonExistentAttributes = cachedAttributes.OfTypeAll<DescriptionAttribute>();

        // Assert
        _ = nonExistentAttributes.Should().BeEmpty();
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
        _ = displayAttributes.Should().NotBeEmpty();
        _ = displayAttributes.Should().HaveCount(1);
        var displayAttribute = displayAttributes[0].Should().BeOfType<DisplayAttribute>().Subject;
        _ = displayAttribute.Name.Should().Be("PropertyWithAttributes_Display");
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
        _ = nonExistentAttributes.Should().BeEmpty();
    }

    private sealed class DummyClass
    {
        [Display(Name = "PropertyWithAttributes_Display")]
        public string? PropertyWithAttributes { get; set; }
    }
}
