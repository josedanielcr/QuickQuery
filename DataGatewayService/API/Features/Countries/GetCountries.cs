using API.Contracts;
using API.Database;
using API.Features.Countries;
using API.Shared;
using Carter;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace API.Features.Countries
{
    public static class GetCountries
    {
        public class Query : IRequest<Result<List<CountriesResponse>>>{ }

        internal sealed class Hanlder : IRequestHandler<Query, Result<List<CountriesResponse>>>
        {
            private readonly ApplicationDbContext dbContext;

            public Hanlder(ApplicationDbContext dbContext)
            {
                this.dbContext = dbContext;
            }

            public async Task<Result<List<CountriesResponse>>> Handle(Query request, CancellationToken cancellationToken)
            {
                return await dbContext.Countries
                    .AsNoTracking()
                    .Select(x => new CountriesResponse
                    {
                        Id = x.Id,
                        Name = x.Name,
                        CostOfLivingIndex = x.CostOfLivingIndex,
                        RentIndex = x.RentIndex,
                        CostOfLivingPlusRentIndex = x.CostOfLivingPlusRentIndex,
                        GroceriesIndex = x.GroceriesIndex,
                        RestaurantPriceIndex = x.RestaurantPriceIndex,
                        LocalPurchasingPowerIndex = x.LocalPurchasingPowerIndex,
                        CreatedAt = x.CreatedAt,
                        UpdatedAt = x.UpdatedAt,
                        Popularity = x.Popularity
                    })
                    .ToListAsync(cancellationToken);
            }
        }
    }
}

public class GetCountriesEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/countries", async (ISender sender) =>
        {
            var query = new GetCountries.Query { };
            var result = await sender.Send(query);

            if (result.IsFailure)
            {
                return Results.BadRequest(result.Error);
            }
            return Results.Ok(result.Value);
        }).RequireAuthorization();
    }
}