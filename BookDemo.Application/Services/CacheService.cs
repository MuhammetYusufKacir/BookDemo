using BookDemo.Core.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace BookDemo.Application.Services
{
    public class CacheService : ICacheService
    {
        private readonly IMemoryCache _cache;

        public CacheService(IMemoryCache cache)
        {
            _cache = cache;
        }

        public async Task<T?> GetAsync<T>(string key)
        {
            return await Task.FromResult(_cache.TryGetValue(key, out T value) ? value : default);
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan expiration)
        {
            var cacheOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = expiration
            };
            _cache.Set(key, value, cacheOptions);
            await Task.CompletedTask;
        }

        public async Task RemoveAsync(string key)
        {
            _cache.Remove(key);
            await Task.CompletedTask;
        }
    }

}
