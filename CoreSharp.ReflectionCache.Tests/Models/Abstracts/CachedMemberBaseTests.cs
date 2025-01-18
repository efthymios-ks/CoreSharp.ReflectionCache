using CoreSharp.ReflectionCache.Models.Abstracts;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace CoreSharp.ReflectionCache.Tests.Models.Abstracts;

public sealed class CachedMemberBaseTests
{
    [Fact]
    public void Constructor_WhenMemberInfoIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        MemberInfo? memberInfo = null;

        // Act
        void Action()
            => _ = new DummyCachedMember(memberInfo);

        // Assert
        Assert.Throws<ArgumentNullException>(Action);
    }

    [Fact]
    public void DebuggerDisplay_WhenCalled_ShouldReturnName()
    {
        // Arrange
        var memberInfo = typeof(DummyClass)
            .GetProperty(nameof(DummyClass.Property1));
        var cachedMember = new DummyCachedMember(memberInfo);

        // Act
        var debuggerDisplay = cachedMember
           .GetType()
           .BaseType
           !.GetProperty("DebuggerDisplay", BindingFlags.NonPublic | BindingFlags.Instance)
           ?.GetValue(cachedMember) as string;

        // Assert
        Assert.Equal(nameof(DummyClass.Property1), debuggerDisplay);
    }

    [Fact]
    public void MemberInfo_WhenCalled_ShouldReturnCorrectValue()
    {
        // Arrange
        var memberInfo = typeof(DummyClass)
            .GetProperty(nameof(DummyClass.Property1));
        var cachedMember = new DummyCachedMember(memberInfo);

        // Act
        var memberInfoRead = cachedMember
            .GetType()
            .GetProperty("MemberInfo", BindingFlags.NonPublic | BindingFlags.Instance)
            ?.GetValue(cachedMember) as PropertyInfo;

        // Assert
        Assert.Same(memberInfo, memberInfoRead);
    }

    [Fact]
    public void Name_WhenCalled_ShouldReturnCorrectValue()
    {
        // Arrange
        var memberInfo = typeof(DummyClass)
            .GetProperty(nameof(DummyClass.Property1));
        var cachedMember = new DummyCachedMember(memberInfo);

        // Act
        var name = cachedMember.Name;

        // Assert
        Assert.Equal(nameof(DummyClass.Property1), name);
    }

    [Fact]
    public void Attributes_WhenCalled_ShouldReturnCorrectValue()
    {
        // Arrange
        var memberInfo = typeof(DummyClass)
            .GetProperty(nameof(DummyClass.Property1));
        var cachedMember = new DummyCachedMember(memberInfo);

        // Act
        var attributes = cachedMember.Attributes;

        // Assert
        Assert.NotNull(attributes);
        Assert.Single(attributes);
        var displayAttribute = Assert.IsType<DisplayAttribute>(attributes.First());
        Assert.Equal("Property1_Display", displayAttribute.Name);
    }

    private class DummyCachedMember(MemberInfo? memberInfo)
        : CachedMemberBase<MemberInfo>(memberInfo)
    {
    }

    private class DummyClass
    {
        [Display(Name = "Property1_Display")]
        public string? Property1 { get; set; } = null;
    }
}
