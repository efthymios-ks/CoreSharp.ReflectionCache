using CoreSharp.ReflectionCache.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace CoreSharp.ReflectionCache.Tests.Models;

public sealed class CachedAttributesTests
{
    [Fact]
    public void Constructor_WhenMemberInfoIsNotNull_ShouldInitializeAttributes()
    {
        // Arrange
        var memberInfo = typeof(DummyClass)
            .GetProperty(nameof(DummyClass.PropertyWithAttributes));

        // Act
        var cachedAttributes = new CachedAttributes(memberInfo);

        // Assert
        Assert.NotNull(cachedAttributes);
        Assert.Single(cachedAttributes);
        var displayAttribute = Assert.IsType<DisplayAttribute>(cachedAttributes.First());
        Assert.Equal("PropertyWithAttributes_Display", displayAttribute.Name);
    }

    [Fact]
    public void Constructor_WhenMemberInfoIsNull_ShouldInitializeEmptyAttributes()
    {
        // Arrange
        MemberInfo? memberInfo = null;

        // Act
        var cachedAttributes = new CachedAttributes(memberInfo);

        // Assert
        Assert.NotNull(cachedAttributes);
        Assert.Empty(cachedAttributes);
    }

    [Fact]
    public void Constructor_WhenAttributesIsNotNull_ShouldInitializeAttributes()
    {
        // Arrange
        var displayAttribute = new DisplayAttribute { Name = "PropertyWithAttributes_Display" };
        var attributes = new Attribute[] { displayAttribute };

        // Act
        var cachedAttributes = new CachedAttributes(attributes);

        // Assert
        Assert.NotNull(cachedAttributes);
        Assert.Single(cachedAttributes);
        var attribute = cachedAttributes.First();
        Assert.Equal(displayAttribute, attribute);
    }

    [Fact]
    public void Constructor_WhenAttributesIsNull_ShouldInitializeEmptyAttributes()
    {
        // Arrange
        Attribute[]? attributes = null;

        // Act
        var cachedAttributes = new CachedAttributes(attributes);

        // Assert
        Assert.NotNull(cachedAttributes);
        Assert.Empty(cachedAttributes);
    }

    [Fact]
    public void OfType_Generic_WhenAttributeExists_ShouldReturnAttribute()
    {
        // Arrange
        var propertyInfo = typeof(DummyClass)
            .GetProperty(nameof(DummyClass.PropertyWithAttributes));
        var cachedAttributes = new CachedAttributes(propertyInfo);

        // Act
        var displayAttribute = cachedAttributes.OfType<DisplayAttribute>();

        // Assert
        Assert.NotNull(displayAttribute);
        Assert.Equal("PropertyWithAttributes_Display", displayAttribute!.Name);
    }

    [Fact]
    public void OfType_Generic_WhenAttributeDoesNotExist_ShouldReturnNull()
    {
        // Arrange
        var propertyInfo = typeof(DummyClass)
            .GetProperty(nameof(DummyClass.PropertyWithAttributes));
        var cachedAttributes = new CachedAttributes(propertyInfo);

        // Act
        var debuggerDisplayAttribute = cachedAttributes.OfType<DescriptionAttribute>();

        // Assert
        Assert.Null(debuggerDisplayAttribute);
    }

    [Fact]
    public void OfType_WhenAttributeExists_ShouldReturnAttribute()
    {
        // Arrange
        var propertyInfo = typeof(DummyClass)
            .GetProperty(nameof(DummyClass.PropertyWithAttributes));
        var cachedAttributes = new CachedAttributes(propertyInfo);

        // Act
        var attribute = cachedAttributes.OfType(typeof(DisplayAttribute));

        // Assert
        Assert.NotNull(attribute);
        var displayAttribute = Assert.IsType<DisplayAttribute>(attribute);
        Assert.Equal("PropertyWithAttributes_Display", displayAttribute.Name);
    }

    [Fact]
    public void OfType_WhenAttributeDoesNotExist_ShouldReturnNull()
    {
        // Arrange
        var propertyInfo = typeof(DummyClass)
            .GetProperty(nameof(DummyClass.PropertyWithAttributes));
        var cachedAttributes = new CachedAttributes(propertyInfo);

        // Act
        var debuggerDisplayAttribute = cachedAttributes.OfType(typeof(DescriptionAttribute));

        // Assert
        Assert.Null(debuggerDisplayAttribute);
    }

    [Fact]
    public void OfTypeAll_Generic_WhenAttributesExist_ShouldReturnAllAttributes()
    {
        // Arrange
        var propertyInfo = typeof(DummyClass)
            .GetProperty(nameof(DummyClass.PropertyWithAttributes));
        var cachedAttributes = new CachedAttributes(propertyInfo);

        // Act
        var displayAttributes = cachedAttributes.OfTypeAll<DisplayAttribute>();

        // Assert
        Assert.NotEmpty(displayAttributes);
        Assert.Single(displayAttributes);
        Assert.Equal("PropertyWithAttributes_Display", displayAttributes[0].Name);
    }

    [Fact]
    public void OfTypeAll_Generic_WhenAttributesDoNotExist_ShouldReturnEmptyArray()
    {
        // Arrange
        var propertyInfo = typeof(DummyClass)
            .GetProperty(nameof(DummyClass.PropertyWithAttributes));
        var cachedAttributes = new CachedAttributes(propertyInfo);

        // Act
        var nonExistentAttributes = cachedAttributes.OfTypeAll<DescriptionAttribute>();

        // Assert
        Assert.Empty(nonExistentAttributes);
    }

    [Fact]
    public void OfTypeAll_WhenAttributesExist_ShouldReturnAllAttributes()
    {
        // Arrange
        var propertyInfo = typeof(DummyClass)
            .GetProperty(nameof(DummyClass.PropertyWithAttributes));
        var cachedAttributes = new CachedAttributes(propertyInfo);

        // Act
        var displayAttributes = cachedAttributes.OfTypeAll(typeof(DisplayAttribute));

        // Assert
        Assert.NotEmpty(displayAttributes);
        Assert.Single(displayAttributes);
        var displayAttribute = Assert.IsType<DisplayAttribute>(displayAttributes[0]);
        Assert.Equal("PropertyWithAttributes_Display", displayAttribute.Name);
    }

    [Fact]
    public void OfTypeAll_WhenAttributesDoNotExist_ShouldReturnEmptyArray()
    {
        // Arrange
        var propertyInfo = typeof(DummyClass)
            .GetProperty(nameof(DummyClass.PropertyWithAttributes));
        var cachedAttributes = new CachedAttributes(propertyInfo);

        // Act
        var nonExistentAttributes = cachedAttributes.OfTypeAll(typeof(DescriptionAttribute));

        // Assert
        Assert.Empty(nonExistentAttributes);
    }

    private sealed class DummyClass
    {
        [Display(Name = "PropertyWithAttributes_Display")]
        public string? PropertyWithAttributes { get; set; }
    }
}
