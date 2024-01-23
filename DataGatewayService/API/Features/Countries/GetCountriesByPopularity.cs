using API.Contracts;
using API.Database;
using API.Features.Countries;
using API.Shared;
using Carter;
using MediatR;

namespace API.Features.Countries
{
    public static class GetCountriesByPopularity
    {
        public class Query : IRequest<Result<CountriesResponse>>{ }

        internal sealed class Hanlder : IRequestHandler<Query, Result<CountriesResponse>>
        {
            private readonly ApplicationDbContext dbContext;

            public Hanlder(ApplicationDbContext dbContext)
            {
                this.dbContext = dbContext;
            }

            public async Task<Result<CountriesResponse>> Handle(Query request, CancellationToken cancellationToken)
            {
                throw new NotImplementedException();
            }
        }
    }
}

public class GetCountriesByPopularityEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/countries", async (ISender sender) =>
        {
            var query = new GetCountriesByPopularity.Query { };
            var result = await sender.Send(query);

            if (result.IsFailure)
            {
                return Results.BadRequest(result.Error);
            }
            return Results.Ok(result.Value);
        }).RequireAuthorization();
    }
}