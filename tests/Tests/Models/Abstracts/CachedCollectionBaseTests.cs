using CoreSharp.ReflectionCache.Models.Abstracts;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace Tests.Models.Abstracts;

[TestFixture]
public sealed class CachedCollectionBaseTests
{
    [Test]
    public void Constructor_WhenSourceIsNotNull_ShouldBeInitializedFromSource()
    {
        // Arrange
        var source = new[] { 1, 2, 3 };

        // Act
        var cachedCollection = new DummyCachedCollection(source);

        // Assert
        cachedCollection.Should().BeEquivalentTo(source);
    }

    [Test]
    public void Constructor_WhenSourceIsNull_ShouldNotThrow()
    {
        // Arrange
        IEnumerable<int> source = null;

        // Act
        Action action = () => _ = new DummyCachedCollection(source);

        // Assert
        action.Should().NotThrow();
    }

    [Test]
    public void Constructor_WhenSourceIsNull_ShouldBeInitializedAsEmpty()
    {
        // Arrange
        IEnumerable<int> source = null;

        // Act
        var cachedCollection = new DummyCachedCollection(source);

        // Assert
        cachedCollection.Should().NotBeNull();
        cachedCollection.Should().BeEmpty();
    }

    [Test]
    public void DebuggerDisplay_WhenCalled_ShouldReturnCount()
    {
        // Arrange
        var source = new[] { 1, 2, 3 };
        var cachedCollection = new DummyCachedCollection(source);

        // Act
        var debuggerDisplay = (string)cachedCollection
            .GetType()
            .BaseType
            .GetProperty("DebuggerDisplay", BindingFlags.NonPublic | BindingFlags.Instance)
            .GetValue(cachedCollection);

        // Assert
        debuggerDisplay.Should().Be("Count=3");
    }

    [Test]
    public void Source_WhenCalled_ShouldReturnItems()
    {
        // Arrange
        var source = new int[] { 1, 2, 3, };
        var cachedCollection = new DummyCachedCollection(source);

        // Act
        var sourceRead = (int[])cachedCollection
            .GetType()
            .GetProperty("Source", BindingFlags.NonPublic | BindingFlags.Instance)
            .GetValue(cachedCollection);

        // Assert
        sourceRead.Should().BeEquivalentTo(source);
    }

    [Test]
    public void Count_WhenCalled_ShouldReturnItemCount()
    {
        // Arrange
        var source = new int[] { 1, 2, 3, };
        var cachedCollection = new DummyCachedCollection(source);

        // Act
        var count = cachedCollection.Count;

        // Assert
        count.Should().Be(source.Length);
    }

    [Test]
    public void GetEnumerator_Generic_WhenCalled_ShouldReturnItemsEnumerator()
    {
        // Arrange
        var source = new int[] { 1, 2, 3, };
        var cachedCollection = new DummyCachedCollection(source);

        // Act
        var enumerator = cachedCollection.GetEnumerator();
        var enumeratorItems = new List<int>();
        while (enumerator.MoveNext())
        {
            enumeratorItems.Add(enumerator.Current);
        }

        // Assert
        enumerator.Should().NotBeNull();
        enumeratorItems.Should().BeEquivalentTo(source);
    }

    [Test]
    public void GetEnumerator_WhenCalled_ShouldReturnItemsEnumerator()
    {
        // Arrange
        var source = new int[] { 1, 2, 3, };
        var cachedCollection = new DummyCachedCollection(source);

        // Act
        var enumerator = ((IEnumerable)cachedCollection).GetEnumerator();
        var enumeratorItems = new List<int>();
        while (enumerator.MoveNext())
        {
            enumeratorItems.Add((int)enumerator.Current);
        }

        // Assert
        enumerator.Should().NotBeNull();
        enumeratorItems.Should().BeEquivalentTo(source);
    }

    private sealed class DummyCachedCollection : CachedCollectionBase<int>
    {
        public DummyCachedCollection(IEnumerable<int> source)
            : base(source)
        {
        }
    }
}
