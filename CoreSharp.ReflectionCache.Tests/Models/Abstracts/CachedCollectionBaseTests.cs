using CoreSharp.ReflectionCache.Models.Abstracts;
using System.Collections;
using System.Reflection;

namespace CoreSharp.ReflectionCache.Tests.Models.Abstracts;

public sealed class CachedCollectionBaseTests
{
    [Fact]
    public void Constructor_WhenSourceIsNotNull_ShouldBeInitializedFromSource()
    {
        // Arrange
        var source = new int[] { 1, 2, 3 };

        // Act
        var cachedCollection = new DummyCachedCollection(source);

        // Assert
        Assert.Equivalent(source, cachedCollection);
    }

    [Fact]
    public void Constructor_WhenSourceIsNull_ShouldNotThrow()
    {
        // Arrange
        IEnumerable<int>? source = null;

        // Act
        void Action()
            => _ = new DummyCachedCollection(source);

        // Assert
        var exception = Record.Exception(Action);
        Assert.Null(exception);
    }

    [Fact]
    public void Constructor_WhenSourceIsNull_ShouldBeInitializedAsEmpty()
    {
        // Arrange
        IEnumerable<int>? source = null;

        // Act
        var cachedCollection = new DummyCachedCollection(source);

        // Assert
        Assert.NotNull(cachedCollection);
        Assert.Empty(cachedCollection);
    }

    [Fact]
    public void DebuggerDisplay_WhenCalled_ShouldReturnCount()
    {
        // Arrange
        var source = new int[] { 1, 2, 3 };
        var cachedCollection = new DummyCachedCollection(source);

        // Act
        var debuggerDisplay = (string?)cachedCollection
            .GetType()
            .BaseType!
            .GetProperty("DebuggerDisplay", BindingFlags.NonPublic | BindingFlags.Instance)
            ?.GetValue(cachedCollection);

        // Assert
        Assert.Equal("Count=3", debuggerDisplay);
    }

    [Fact]
    public void Source_WhenCalled_ShouldReturnItems()
    {
        // Arrange
        var source = new[] { 1, 2, 3 };
        var cachedCollection = new DummyCachedCollection(source);

        // Act
        var sourceRead = (int[]?)cachedCollection
            .GetType()
            .GetProperty("Source", BindingFlags.NonPublic | BindingFlags.Instance)
            ?.GetValue(cachedCollection);

        // Assert
        Assert.Equivalent(source, sourceRead);
    }

    [Fact]
    public void Count_WhenCalled_ShouldReturnItemCount()
    {
        // Arrange
        var source = new[] { 1, 2, 3 };
        var cachedCollection = new DummyCachedCollection(source);

        // Act
        var count = cachedCollection.Count;

        // Assert
        Assert.Equal(source.Length, count);
    }

    [Fact]
    public void GetEnumerator_Generic_WhenCalled_ShouldReturnItemsEnumerator()
    {
        // Arrange
        var source = new[] { 1, 2, 3 };
        var cachedCollection = new DummyCachedCollection(source);

        // Act
        var enumerator = cachedCollection.GetEnumerator();
        var enumeratorItems = new List<int>();
        while (enumerator.MoveNext())
        {
            enumeratorItems.Add(enumerator.Current);
        }

        // Assert
        Assert.NotNull(enumerator);
        Assert.Equivalent(source, enumeratorItems);
    }

    [Fact]
    public void GetEnumerator_WhenCalled_ShouldReturnItemsEnumerator()
    {
        // Arrange
        var source = new[] { 1, 2, 3 };
        var cachedCollection = new DummyCachedCollection(source);

        // Act
        var enumerator = ((IEnumerable)cachedCollection).GetEnumerator();
        var enumeratorItems = new List<int>();
        while (enumerator.MoveNext())
        {
            enumeratorItems.Add((int)enumerator.Current);
        }

        // Assert
        Assert.NotNull(enumerator);
        Assert.Equivalent(source, enumeratorItems);
    }

    private sealed class DummyCachedCollection(IEnumerable<int>? source)
        : CachedCollectionBase<int>(source)
    {
    }
}
