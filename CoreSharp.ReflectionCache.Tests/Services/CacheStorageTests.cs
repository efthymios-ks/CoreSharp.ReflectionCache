using CoreSharp.ReflectionCache.Services;
using Microsoft.Extensions.Caching.Memory;

namespace CoreSharp.ReflectionCache.Tests.Services;

public sealed class CacheStorageTests
{
    [Fact]
    public void Constructor_WhenCalled_ShouldNotThrow()
    {
        // Arrange
        var memoryCache = Substitute.For<IMemoryCache>();

        // Act
        void Action()
            => _ = new CacheStorage(memoryCache);

        // Assert
        var exception = Record.Exception(Action);
        Assert.Null(exception);
    }

    [Fact]
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
        int ValueFactory()
        {
            valueFactoredCalled = true;
            return valueFactoryResult;
        }

        // Act 
        var result = cacheStorage.GetOrAdd(cacheKey, ValueFactory, TimeSpan.FromMinutes(1));

        // Assert
        Assert.Equal(cachedValue, result);
        Assert.False(valueFactoredCalled);
    }

    [Fact]
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
        int ValueFactory()
        {
            valueFactoredCalled = true;
            return valueFactoryResult;
        }

        // Act 
        var result = cacheStorage.GetOrAdd(cacheKey, ValueFactory, TimeSpan.FromMinutes(1));

        // Assert
        Assert.Equal(valueFactoryResult, result);
        Assert.True(valueFactoredCalled);
    }
}
