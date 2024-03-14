using QuickquerySearchAPI.Shared;
using Microsoft.Extensions.Caching.Distributed;
using QuickquerySearchAPI.Resources.Cache;

namespace QuickquerySearchAPI.Utilities
{
    public class CacheUtils
    {
        private readonly IDistributedCache cache;

        public CacheUtils(IDistributedCache cache)
        {
            this.cache = cache;
        }

        public async Task<Result<string>> GetCacheValueAsync(string key)
        {
            string? result = await cache.GetStringAsync(key);

            if (result is null)
            {
                return Result.Failure<string>(
                    new Error(CacheCodeMessages.CacheNotFound,
                    string.Format(CacheMessages.Cache_NotFound, key)));
            }
            return result;
        }

        public async Task<Result> SetCacheValueAsync(string key, string value, TimeSpan? expiry = null)
        {
            await cache.SetStringAsync(key, value, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = expiry
            });
            return Result.Success();
        }

        public async Task<Result> RemoveCacheValueAsync(string key)
        {
            await cache.RemoveAsync(key);
            return Result.Success();
        }

        public async Task<Result> UpdateCacheExpiryAsync(string key, TimeSpan expiry)
        {
            var value = await GetCacheValueAsync(key);
            if (value.IsFailure)
            {
                return value;
            }
            await SetCacheValueAsync(key, value.Value, expiry);
            return Result.Success();
        }
    }
}