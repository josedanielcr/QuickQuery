using QuickquerySearchAPI.Contracts;
using QuickquerySearchAPI.Features.Search;
using QuickquerySearchAPI.Shared;
using QuickquerySearchAPI.Utilities;
using Carter;
using MediatR;
using Microsoft.Extensions.Primitives;
using QuickquerySearchAPI.Resources.Cache;

namespace QuickquerySearchAPI.Features.Search
{
    public class SearchByCountryName
    {
        public class Query : IRequest<Result<CountrySearchResult>>
        {
            public IDictionary<string, StringValues> Headers { get; set; } = null!;
            public string Name { get; set; } = string.Empty;
        }

        internal sealed class Handler : IRequestHandler<Query, Result<CountrySearchResult>>
        {
            private readonly SearchCountryCacheUtils searchCountryCacheUtils;
            private readonly SearchCountryHttpUtils searchCountryHttpUtils;
            private readonly CountrySearchLogUtils countrySearchLogUtils;

            public Handler(SearchCountryCacheUtils searchCountryCacheUtils,
                SearchCountryHttpUtils searchCountryHttpUtils,
                CountrySearchLogUtils countrySearchLogUtils)
            {
                this.searchCountryCacheUtils = searchCountryCacheUtils;
                this.searchCountryHttpUtils = searchCountryHttpUtils;
                this.countrySearchLogUtils = countrySearchLogUtils;
            }

            public async Task<Result<CountrySearchResult>> Handle(Query request, CancellationToken cancellationToken)
            {
                var cacheResult = await GetCachedCountryData(request);
                if (cacheResult.IsSuccess)
                {
                    return await ProcessSuccessfulResult(request, cacheResult);
                }

                if (cacheResult.IsFailure && cacheResult.Error.Code != CacheCodeMessages.CacheNotFound)
                {
                    return cacheResult;
                }

                var httpResult = await FetchDataFromHttpService(request);
                if (httpResult.IsSuccess)
                {
                    await searchCountryCacheUtils.SetCountryResponseToCache(httpResult);
                    return await ProcessSuccessfulResult(request, httpResult);
                }

                return httpResult;
            }

            private async Task<Result<CountrySearchResult>> ProcessSuccessfulResult(Query request,
                Result<CountrySearchResult> result)
            {
                var popularityResult 
                    = await searchCountryHttpUtils.IncreaseCountryPropularity(result, request.Headers);
                
                if (popularityResult.IsFailure)
                {
                    return Result.Failure<CountrySearchResult>(popularityResult.Error);
                }
                _ = countrySearchLogUtils.LogCountrySearch(request, result);
                return popularityResult;
            }

            private async Task<Result<CountrySearchResult>> GetCachedCountryData(Query request)
            {
                return await searchCountryCacheUtils.GetCountryDataFromCache(request);
            }

            private async Task<Result<CountrySearchResult>> FetchDataFromHttpService(Query request)
            {
                return await searchCountryHttpUtils.GetCountryDataFromDataGatewayService(request, request.Headers);
            }
        }
    }
}

public class GetCountryDataByNameEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/search/country", async (HttpContext httpContext, string name, ISender sender) =>
        {
            var query = new SearchByCountryName.Query 
            { 
                Name = name,
                Headers = httpContext.Request.Headers.ToDictionary(x => x.Key, x => x.Value)
            };
            var result = await sender.Send(query);

            if (result.IsFailure)
            {
                return Results.BadRequest(result.Error);
            }
            return Results.Ok(result.Value);
        }).RequireAuthorization();
    }
}