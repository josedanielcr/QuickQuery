using API.Contracts;
using API.Features.Search;
using API.Shared;
using API.Utilities;
using Carter;
using MediatR;
using Newtonsoft.Json;

namespace API.Features.Search
{
    public class SearchByCountryName
    {
        public class Query : IRequest<Result<CountrySearchResult>>
        {
            public string Name { get; set; }
        }

        internal sealed class Handler : IRequestHandler<Query, Result<CountrySearchResult>>
        {
            private readonly SearchCountryCacheUtils searchCountryCacheUtils;
            private readonly SearchCountryHttpUtils searchCountryHttpUtils;

            public Handler(SearchCountryCacheUtils searchCountryCacheUtils,
                SearchCountryHttpUtils searchCountryHttpUtils)
            {
                this.searchCountryCacheUtils = searchCountryCacheUtils;
                this.searchCountryHttpUtils = searchCountryHttpUtils;
            }

            public async Task<Result<CountrySearchResult>> Handle(Query request, CancellationToken cancellationToken)
            {
                Result<CountrySearchResult> result;
                result = await searchCountryCacheUtils.GetCountryDataFromCache(request);
                if (result.IsSuccess)
                {
                    return result;
                }

                result = await searchCountryHttpUtils.GetCountryDataFromDataGatewayService(request);
                if (result.IsSuccess)
                {
                    searchCountryCacheUtils.SetCountryResponseToCache(result);
                }
                return result;  
            }
        }
    }
}

public class GetCountryDataByNameEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/search/country", async (string name, ISender sender) =>
        {
            var query = new SearchByCountryName.Query { Name = name };
            var result = await sender.Send(query);

            if (result.IsFailure)
            {
                return Results.BadRequest(result.Error);
            }
            return Results.Ok(result.Value);
        }).RequireAuthorization();
    }
}