using CoreSharp.ReflectionCache.Models.Abstracts;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace Tests.Models.Abstracts;

[TestFixture]
public sealed class CachedMemberBaseTests
{
    [Test]
    public void Constructor_WhenMemberInfoIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        MemberInfo memberInfo = null;

        // Act
        Action action = () => _ = new DummyCachedMember(memberInfo);

        // Assert
        action.Should().ThrowExactly<ArgumentNullException>();
    }

    [Test]
    public void DebuggerDisplay_WhenCalled_ShouldReturnName()
    {
        // Arrange
        var memberInfo = typeof(DummyClass)
            .GetProperty(nameof(DummyClass.Property1));
        var cachedMember = new DummyCachedMember(memberInfo);

        // Act
        var debuggerDisplay = (string)cachedMember
           .GetType()
           .BaseType
           .GetProperty("DebuggerDisplay", BindingFlags.NonPublic | BindingFlags.Instance)
           .GetValue(cachedMember);

        // Assert
        debuggerDisplay.Should().Be(nameof(DummyClass.Property1));
    }

    [Test]
    public void MemberInfo_WhenCalled_ShouldReturnCorrectValue()
    {
        // Arrange
        var memberInfo = typeof(DummyClass)
            .GetProperty(nameof(DummyClass.Property1));
        var cachedMember = new DummyCachedMember(memberInfo);

        // Act
        var memberInfoRead = (PropertyInfo)cachedMember
            .GetType()
            .GetProperty("MemberInfo", BindingFlags.NonPublic | BindingFlags.Instance)
            .GetValue(cachedMember);

        // Assert
        memberInfoRead.Should().BeSameAs(memberInfo);
    }

    [Test]
    public void Name_WhenCalled_ShouldReturnCorrectValue()
    {
        // Arrange
        var memberInfo = typeof(DummyClass)
            .GetProperty(nameof(DummyClass.Property1));
        var cachedMember = new DummyCachedMember(memberInfo);

        // Act
        var name = cachedMember.Name;

        // Assert
        name.Should().Be(nameof(DummyClass.Property1));
    }

    [Test]
    public void Attributes_WhenCalled_ShouldReturnCorrectValue()
    {
        // Arrange
        var memberInfo = typeof(DummyClass)
            .GetProperty(nameof(DummyClass.Property1));
        var cachedMember = new DummyCachedMember(memberInfo);

        // Act
        var attributes = cachedMember.Attributes;

        // Assert
        attributes.Should().NotBeNull();
        attributes.Should().HaveCount(1);
        var displayAttribute = attributes.First().Should().BeOfType<DisplayAttribute>().Subject;
        displayAttribute.Name.Should().Be("Property1_Display");
    }

    private class DummyCachedMember : CachedMemberBase<MemberInfo>
    {
        public DummyCachedMember(MemberInfo memberInfo)
            : base(memberInfo)
        {
        }
    }

    private class DummyClass
    {
        [Display(Name = "Property1_Display")]
        public string Property1 { get; set; }
    }
}
