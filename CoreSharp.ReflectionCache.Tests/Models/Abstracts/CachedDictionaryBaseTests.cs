using CoreSharp.ReflectionCache.Models.Abstracts;
using System.Collections;
using System.Reflection;

namespace CoreSharp.ReflectionCache.Tests.Models.Abstracts;

public sealed class CachedDictionaryBaseTests
{
    [Fact]
    public void Constructor_WhenSourceIsNotNull_ShouldBeInitializedFromSource()
    {
        // Arrange
        var source = new Dictionary<string, string>
        {
            { "One", "1" },
            { "Two", "2" },
            { "Three", "3" }
        };

        // Act
        var cachedDictionary = new DummyCachedDictionary(source);

        // Assert
        Assert.Equivalent(source, cachedDictionary);
    }

    [Fact]
    public void Constructor_WhenSourceIsNull_ShouldNotThrow()
    {
        // Arrange
        IReadOnlyDictionary<string, string>? source = null;

        // Act
        void Action()
            => _ = new DummyCachedDictionary(source);

        // Assert
        var exception = Record.Exception(Action);
        Assert.Null(exception);
    }

    [Fact]
    public void Constructor_WhenSourceIsNull_ShouldBeInitializedAsEmpty()
    {
        // Arrange
        IReadOnlyDictionary<string, string>? source = null;

        // Act
        var cachedDictionary = new DummyCachedDictionary(source);

        // Assert
        Assert.NotNull(cachedDictionary);
        Assert.Empty(cachedDictionary);
    }

    [Fact]
    public void DebuggerDisplay_WhenCalled_ShouldReturnCount()
    {
        // Arrange
        var source = new Dictionary<string, string>
        {
            { "One", "1" },
            { "Two", "2" },
            { "Three", "3" }
        };
        var cachedDictionary = new DummyCachedDictionary(source);

        // Act
        var debuggerDisplay = cachedDictionary
           .GetType()
           .BaseType
           !.GetProperty("DebuggerDisplay", BindingFlags.NonPublic | BindingFlags.Instance)
           ?.GetValue(cachedDictionary) as string;

        // Assert
        Assert.Equal("Count=3", debuggerDisplay);
    }

    [Fact]
    public void Indexer_WhenCalled_ShouldReturnCorrectValue()
    {
        // Arrange
        var source = new Dictionary<string, string>
        {
            { "One", "1" },
            { "Two", "2" },
            { "Three", "3" }
        };
        var cachedDictionary = new DummyCachedDictionary(source);

        // Act
        var value = cachedDictionary["Two"];

        // Assert
        Assert.Equal("2", value);
    }

    [Fact]
    public void Keys_WhenCalled_ShouldReturnKeys()
    {
        // Arrange
        var source = new Dictionary<string, string>
        {
            { "One", "1" },
            { "Two", "2" },
            { "Three", "3" }
        };
        var cachedDictionary = new DummyCachedDictionary(source);

        // Act
        var keys = cachedDictionary.Keys;

        // Assert
        Assert.Equivalent(source.Keys, keys);
    }

    [Fact]
    public void Values_WhenCalled_ShouldReturnValues()
    {
        // Arrange
        var source = new Dictionary<string, string>
        {
            { "One", "1" },
            { "Two", "2" },
            { "Three", "3" }
        };
        var cachedDictionary = new DummyCachedDictionary(source);

        // Act
        var values = cachedDictionary.Values;

        // Assert
        Assert.Equivalent(source.Values, values);
    }

    [Fact]
    public void Count_WhenCalled_ShouldReturnItemsCount()
    {
        // Arrange
        var source = new Dictionary<string, string>
        {
            { "One", "1" },
            { "Two", "2" },
            { "Three", "3" }
        };
        var cachedDictionary = new DummyCachedDictionary(source);

        // Act
        var count = cachedDictionary.Count;

        // Assert
        Assert.Equal(source.Count, count);
    }

    [Fact]
    public void ContainsKey_WhenKeyExists_ShouldReturnTrue()
    {
        // Arrange
        var source = new Dictionary<string, string>
        {
            { "One", "1" },
            { "Two", "2" },
            { "Three", "3" }
        };
        var cachedDictionary = new DummyCachedDictionary(source);

        // Act
        var exists = cachedDictionary.ContainsKey("Two");

        // Assert
        Assert.True(exists);
    }

    [Fact]
    public void ContainsKey_WhenKeyDoesNotExist_ShouldReturnFalse()
    {
        // Arrange
        var source = new Dictionary<string, string>
        {
            { "One", "1" },
            { "Two", "2" },
            { "Three", "3" }
        };
        var cachedDictionary = new DummyCachedDictionary(source);

        // Act
        var exists = cachedDictionary.ContainsKey("Four");

        // Assert
        Assert.False(exists);
    }

    [Fact]
    public void TryGetValue_WhenKeyExists_ShouldReturnTrueAndValue()
    {
        // Arrange
        var source = new Dictionary<string, string>
        {
            { "One", "1" },
            { "Two", "2" },
            { "Three", "3" }
        };
        var cachedDictionary = new DummyCachedDictionary(source);

        // Act
        var exists = cachedDictionary.TryGetValue("Two", out var value);

        // Assert
        Assert.True(exists);
        Assert.Equal("2", value);
    }

    [Fact]
    public void TryGetValue_WhenKeyDoesNotExist_ShouldReturnFalseAndDefault()
    {
        // Arrange
        var source = new Dictionary<string, string>
        {
            { "One", "1" },
            { "Two", "2" },
            { "Three", "3" }
        };
        var cachedDictionary = new DummyCachedDictionary(source);

        // Act
        var exists = cachedDictionary.TryGetValue("Four", out var value);

        // Assert
        Assert.False(exists);
        Assert.Null(value);
    }

    [Fact]
    public void GetEnumerator_Generic_WhenCalled_ShouldReturnItemsEnumerator()
    {
        // Arrange
        var source = new Dictionary<string, string>
        {
            { "One", "1" },
            { "Two", "2" },
            { "Three", "3" }
        };
        var cachedDictionary = new DummyCachedDictionary(source);

        // Act
        var enumerator = cachedDictionary.GetEnumerator();
        var enumeratedItems = new List<KeyValuePair<string, string>>();
        while (enumerator.MoveNext())
        {
            enumeratedItems.Add(enumerator.Current);
        }

        // Assert 
        Assert.NotNull(enumerator);
        Assert.Equivalent(source, enumeratedItems);
    }

    [Fact]
    public void GetEnumerator_WhenCalled_ShouldReturnItemsEnumerator()
    {
        // Arrange
        var source = new Dictionary<string, string>
        {
            { "One", "1" },
            { "Two", "2" },
            { "Three", "3" }
        };
        var cachedDictionary = new DummyCachedDictionary(source);

        // Act
        var enumerator = ((IEnumerable)cachedDictionary).GetEnumerator();
        var enumeratedItems = new List<KeyValuePair<string, string>>();
        while (enumerator.MoveNext())
        {
            enumeratedItems.Add((KeyValuePair<string, string>)enumerator.Current);
        }

        // Assert 
        Assert.NotNull(enumerator);
        Assert.Equivalent(source, enumeratedItems);
    }

    private sealed class DummyCachedDictionary(IReadOnlyDictionary<string, string>? source)
        : CachedDictionaryBase<string>(source)
    {
    }
}
