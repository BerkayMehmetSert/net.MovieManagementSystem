using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace Core.Application.Caching;

public class MemoryCacheService : ICacheService
{
    private readonly IMemoryCache _memoryCache;
    private readonly CacheOptions _cacheConfig;
    private MemoryCacheEntryOptions _cacheOptions;

    public MemoryCacheService(IMemoryCache memoryCache, IOptions<CacheOptions> cacheConfig)
    {
        _memoryCache = memoryCache;
        _cacheConfig = cacheConfig.Value;
        if (_cacheConfig is not null)
        {
            _cacheOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpiration = DateTime.Now.AddHours(_cacheConfig.AbsoluteExpirationInHours),
                Priority = CacheItemPriority.High,
                SlidingExpiration = TimeSpan.FromMinutes(_cacheConfig.SlidingExpirationInMinutes)
            };
        }
    }

    public bool TryGet<T>(string cacheKey, out T value)
    {
        _memoryCache.TryGetValue(cacheKey, out value);
        return value is not null;
    }

    public T Set<T>(string cacheKey, T value) => _memoryCache.Set(cacheKey, value, _cacheOptions);
    public void Remove(string cacheKey) => _memoryCache.Remove(cacheKey);
}