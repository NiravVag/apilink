using Contracts.Managers;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Text;

namespace BI.Cache
{
    public class CacheManager : ICacheManager
    {
        private readonly IMemoryCache _cache;

        public CacheManager(IMemoryCache cache)
        {
            _cache = cache;
        }

        public T CacheTryGetValueSet<T>(string key, Func<T> GetData)
        {
            T data;

            // Look for cache key.
            if (!_cache.TryGetValue<T>(key, out data))
            {
                // Key not in cache, so get data.
                data = GetData();

                // Set cache options.
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    // Keep in cache for this time, reset time if accessed.
                    .SetSlidingExpiration(TimeSpan.FromDays(100));

                // Save data in cache.
                _cache.Set(key, data, cacheEntryOptions);
            }

            return data;
        }

        public void Clear()
        {
            _cache.Remove(CacheKeys.AllCountries);
            _cache.Remove(CacheKeys.AllLocations);
            _cache.Remove(CacheKeys.AllRights);
            _cache.Remove(CacheKeys.AllRoles);
            _cache.Remove(CacheKeys.AllTranslations);
            _cache.Remove(CacheKeys.AllZoneDistance);
            _cache.Remove(CacheKeys.AllZones);
            _cache.Remove(CacheKeys.Instances);
            _cache.Remove(CacheKeys.AllProductCategories);
            _cache.Remove(CacheKeys.AllProductSubCategories);
            _cache.Remove(CacheKeys.AllProductCategorySub2s);
        }
    }
}
