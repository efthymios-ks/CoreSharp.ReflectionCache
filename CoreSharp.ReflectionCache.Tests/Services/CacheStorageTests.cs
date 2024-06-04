using CoreSharp.ReflectionCache.Services;
using FluentAssertions;
using Microsoft.Extensions.Caching.Memory;
using NSubstitute;
using NUnit.Framework;

namespace Tests.Services;

[TestFixture]
public sealed class CacheStorageTests
{
    [Test]
    public void Constructor_WhenCalled_ShouldNotThrow()
    {
        // Arrange
        var memoryCache = Substitute.For<IMemoryCache>();

        // Act
        Action action = () => _ = new CacheStorage(memoryCache);

        // Assert
        action.Should().NotThrow();
    }

    [Test]
    public void GetOrAdd_WhenItemExists_ReturnExisting()
    {
        // Arrange
        var memoryCache = Substitute.For<IMemoryCache>();
        var cacheStorage = new CacheStorage(memoryCache);
        const string cacheKey = "key";
        const int cachedValue = 1;
        memoryCache
            .TryGetValue<int>(cacheKey, out _)
            .ReturnsForAnyArgs(call =>
            {
                call[1] = cachedValue;
                return true;
            });

        const int valueFactoryResult = cachedValue + 1;
        var valueFactoredCalled = false;
        Func<int> valueFactory = () =>
        {
            valueFactoredCalled = true;
            return valueFactoryResult;
        };

        // Act 
        var result = cacheStorage.GetOrAdd<int>(cacheKey, valueFactory, TimeSpan.FromMinutes(1));

        // Assert
        result.Should().Be(cachedValue);
        valueFactoredCalled.Should().BeFalse();
    }

    [Test]
    public void GetOrAdd_WhenItemDoesNotExist_CreateAndReturnNew()
    {
        // Arrange
        var memoryCache = Substitute.For<IMemoryCache>();
        var cacheStorage = new CacheStorage(memoryCache);
        const string cacheKey = "key";
        const int cachedValue = 1;
        memoryCache
            .TryGetValue<int>(cacheKey, out _)
            .ReturnsForAnyArgs(call =>
            {
                call[1] = cachedValue;
                return false;
            });

        const int valueFactoryResult = cachedValue + 1;
        var valueFactoredCalled = false;
        Func<int> valueFactory = () =>
        {
            valueFactoredCalled = true;
            return valueFactoryResult;
        };

        // Act 
        var result = cacheStorage.GetOrAdd<int>(cacheKey, valueFactory, TimeSpan.FromMinutes(1));

        // Assert
        result.Should().Be(valueFactoryResult);
        valueFactoredCalled.Should().BeTrue();
    }
}
