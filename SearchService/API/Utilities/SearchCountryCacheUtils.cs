using API.Contracts;
using API.Shared;
using Newtonsoft.Json;
using static API.Features.Search.SearchByCountryName;

namespace API.Utilities
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
            Result<string> result = await cache.GetCacheValueAsync(request.Name);
            if (result.IsFailure && result.Error.Code == "Cache.NotFound")
            {
                return Result.Failure<CountrySearchResult>(
                           new Error("Cache.NotFound", $"Cache key {request.Name} not found."));
            }
            else if (result.IsFailure)
            {
                return Result.Failure<CountrySearchResult>(result.Error);
            }
            return DeserializeCacheItemToCountrySearchResult(result);
        }

        private Result<CountrySearchResult> DeserializeCacheItemToCountrySearchResult(Result<string> result)
        {
            CountrySearchResult? deserializedCacheItem =
                    JsonConvert.DeserializeObject<CountrySearchResult>(result.Value);

            if (deserializedCacheItem == null)
            {
                return Result.Failure<CountrySearchResult>(
                    new Error("Cache.DeserializeObject",
                        "Failed to deserialize the cached item. The data might be corrupted, incorrectly formatted, or incompatible with the expected object structure."));
            }
            return Result.Success(deserializedCacheItem);
        }

        public async void SetCountryResponseToCache(Result<CountrySearchResult> result)
        {
            var cacheResult = await cache.GetCacheValueAsync(result.Value.Name);
            await ManageCacheCountryResponse(result, cacheResult);
        }

        private async Task ManageCacheCountryResponse(Result<CountrySearchResult> result, Result<string> cacheResult)
        {
            if (cacheResult.IsFailure && cacheResult.Error.Code == "Cache.NotFound")
            {
                await cache.SetCacheValueAsync(result.Value.Name, 
                    JsonConvert.SerializeObject(result.Value),
                    TimeSpan.FromMinutes(double.Parse(
                        configuration.GetSection("CacheOptions:CacheDurationInMinutes").Value!)));
            }
            else
            {
                await UpdateCountryCacheExpirationTime(result);
            }
        }

        private async Task UpdateCountryCacheExpirationTime(Result<CountrySearchResult> result)
        {
            int popularity = result.Value.Popularity;
            int newCacheExpiration = CalculateExpirationTimeBasedOnPopularity(popularity);
            await cache.UpdateCacheExpiryAsync(result.Value.Name,
                TimeSpan.FromMinutes(newCacheExpiration));
        }

        private int CalculateExpirationTimeBasedOnPopularity(int popularity)
        {
            int newCacheExpiration = int.Parse(configuration.GetSection("CacheOptions:CacheDurationInMinutes").Value!)
                + (popularity * int.Parse(configuration.GetSection("CacheOptions:IncrementalFactorInMinutes").Value!));
            newCacheExpiration = ValidateMaxExpirationTime(newCacheExpiration);

            return newCacheExpiration;
        }

        private int ValidateMaxExpirationTime(int newCacheExpiration)
        {
            if (newCacheExpiration > int.Parse(configuration.GetSection("CacheOptions:MaxCacheDurationInMinutes").Value!))
            {
                newCacheExpiration = int.Parse(configuration.GetSection("CacheOptions:MaxCacheDurationInMinutes").Value!);
            }

            return newCacheExpiration;
        }
    }
}
