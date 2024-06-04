using CoreSharp.ReflectionCache.Models.Abstracts;
using FluentAssertions;
using NUnit.Framework;
using System.Collections;
using System.Reflection;

namespace Tests.Models.Abstracts;

[TestFixture]
public sealed class CachedDictionaryBaseTests
{
    [Test]
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
        _ = cachedDictionary.Should().BeEquivalentTo(source);
    }

    [Test]
    public void Constructor_WhenSourceIsNull_ShouldNotThrow()
    {
        // Arrange
        IReadOnlyDictionary<string, string>? source = null;

        // Act
        Action action = () => _ = new DummyCachedDictionary(source);

        // Assert
        _ = action.Should().NotThrow();
    }

    [Test]
    public void Constructor_WhenSourceIsNull_ShouldBeInitializedAsEmpty()
    {
        // Arrange
        IReadOnlyDictionary<string, string>? source = null;

        // Act
        var cachedDictionary = new DummyCachedDictionary(source);

        // Assert
        _ = cachedDictionary.Should().NotBeNull();
        _ = cachedDictionary.Should().BeEmpty();
    }

    [Test]
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
        _ = debuggerDisplay.Should().Be("Count=3");
    }

    [Test]
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
        _ = value.Should().Be("2");
    }

    [Test]
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
        _ = keys.Should().BeEquivalentTo(source.Keys);
    }

    [Test]
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
        _ = values.Should().BeEquivalentTo(source.Values);
    }

    [Test]
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
        _ = count.Should().Be(source.Count);
    }

    [Test]
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
        _ = exists.Should().BeTrue();
    }

    [Test]
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
        _ = exists.Should().BeFalse();
    }

    [Test]
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
        _ = exists.Should().BeTrue();
        _ = value.Should().Be("2");
    }

    [Test]
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
        _ = exists.Should().BeFalse();
        _ = value.Should().BeNull();
    }

    [Test]
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
        _ = enumerator.Should().NotBeNull();
        _ = enumeratedItems.Should().BeEquivalentTo(source);
    }

    [Test]
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
        _ = enumerator.Should().NotBeNull();
        _ = enumeratedItems.Should().BeEquivalentTo(source);
    }

    private sealed class DummyCachedDictionary(IReadOnlyDictionary<string, string>? source)
        : CachedDictionaryBase<string>(source)
    {
    }
}
