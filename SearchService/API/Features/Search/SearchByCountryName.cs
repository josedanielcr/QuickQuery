using API.Contracts;
using API.Features.Search;
using API.Shared;
using API.Utilities;
using Carter;
using MediatR;
using Microsoft.Extensions.Primitives;

namespace API.Features.Search
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
                Result<CountrySearchResult> cacheResult 
                    = await searchCountryCacheUtils.GetCountryDataFromCache(request);
                if (cacheResult.IsSuccess)
                {
                    var result = await searchCountryHttpUtils.IncreaseCountryPropularity(cacheResult, request.Headers);
                    var logResult = await countrySearchLogUtils.LogCountrySearch(request, cacheResult);
                    if (logResult.IsFailure)
                    {
                        return Result.Failure<CountrySearchResult>(logResult.Error);
                    }
                    return result;
                }

                if(cacheResult.IsFailure && cacheResult.Error.Code != "Cache.NotFound") return cacheResult;

                Result<CountrySearchResult> httpResult 
                    = await searchCountryHttpUtils.GetCountryDataFromDataGatewayService(request,request.Headers);
                if (httpResult.IsSuccess)
                {
                    await searchCountryCacheUtils.SetCountryResponseToCache(httpResult);
                    var result = await searchCountryHttpUtils.IncreaseCountryPropularity(httpResult, request.Headers);
                    var logResult = await countrySearchLogUtils.LogCountrySearch(request, httpResult);
                    if (logResult.IsFailure)
                    {
                        return Result.Failure<CountrySearchResult>(logResult.Error);
                    }
                    return result;
                }
                return httpResult;  
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