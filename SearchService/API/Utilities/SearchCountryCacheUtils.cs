using QuickquerySearchAPI.Contracts;
using QuickquerySearchAPI.Shared;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using static QuickquerySearchAPI.Features.Search.SearchByCountryName;
using QuickquerySearchAPI.Resources.Cache;

namespace QuickquerySearchAPI.Utilities
{
    public class SearchCountryCacheUtils
    {
        private readonly CacheUtils cache;
        private readonly IConfiguration configuration;

        public SearchCountryCacheUtils(CacheUtils cache, 
            IConfiguration configuration)
        {
            this.cache = cache;
            this.configuration = configuration;
        }
        public async Task<Result<CountrySearchResult>> GetCountryDataFromCache(Query request)
        {
            Result<string> cacheResult = await cache.GetCacheValueAsync(request.Name);

            if (cacheResult.IsFailure)
            {
                return cacheResult.Error.Code == CacheCodeMessages.CacheNotFound
                   ? Result.Failure<CountrySearchResult>(
                       new Error(CacheCodeMessages.CacheNotFound, 
                        string.Format(CacheMessages.Cache_NotFound,request.Name)))
                   : Result.Failure<CountrySearchResult>(cacheResult.Error);
            }

            return DeserializeCacheItemToCountrySearchResult(cacheResult);
        }

        private Result<CountrySearchResult> DeserializeCacheItemToCountrySearchResult(Result<string> result)
        {
            CountrySearchResult? deserializedCacheItem =
                    JsonConvert.DeserializeObject<CountrySearchResult>(result.Value);

            return deserializedCacheItem != null
                            ? Result.Success(deserializedCacheItem)
                            : Result.Failure<CountrySearchResult>(
                                new Error(CacheCodeMessages.CacheDeserializeError, 
                                string.Format(CacheMessages.Cache_DeserializeObject,result.Value)));
        }

        public async Task SetCountryResponseToCache(Result<CountrySearchResult> result)
        {
            var cacheResult = await cache.GetCacheValueAsync(result.Value.Name);
            if (cacheResult.IsFailure && cacheResult.Error.Code == CacheCodeMessages.CacheNotFound)
            {
                await cache.SetCacheValueAsync(result.Value.Name,
                    JsonConvert.SerializeObject(result.Value),
                    GetCacheDuration());
            }
            else
            {
                await UpdateCountryCacheExpirationTime(result);
            }
        }

        private TimeSpan GetCacheDuration()
        {
            var durationInMinutes = configuration.GetValue<int>("CacheOptions:CacheDurationInMinutes");
            return TimeSpan.FromMinutes(durationInMinutes);
        }

        private async Task UpdateCountryCacheExpirationTime(Result<CountrySearchResult> result)
        {
            var newCacheExpiration 
                = CalculateExpirationTimeBasedOnPopularity(result.Value.Popularity);
            await cache.UpdateCacheExpiryAsync(result.Value.Name, 
                TimeSpan.FromMinutes(newCacheExpiration));
        }

        private int CalculateExpirationTimeBasedOnPopularity(int popularity)
        {
            var baseDuration = configuration.GetValue<int>("CacheOptions:CacheDurationInMinutes");
            var incrementalFactor = configuration.GetValue<int>("CacheOptions:IncrementalFactorInMinutes");
            var maxDuration = configuration.GetValue<int>("CacheOptions:MaxCacheDurationInMinutes");

            var newCacheExpiration = baseDuration + (popularity * incrementalFactor);
            return Math.Min(newCacheExpiration, maxDuration);
        }
    }
}