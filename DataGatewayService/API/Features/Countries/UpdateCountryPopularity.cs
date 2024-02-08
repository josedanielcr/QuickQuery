using API.Contracts;
using API.Database;
using API.Shared;
using Carter;
using MediatR;
using static API.Features.Countries.UpdateCountryPopularity;

namespace API.Features.Countries
{
    public class UpdateCountryPopularity
    {
        public class Query : IRequest<Result<CountryResponse>>
        {
            public Guid Id { get; set; }
        }

        internal sealed class Handler : IRequestHandler<Query, Result<CountryResponse>>
        {
            private readonly ApplicationDbContext dbContext;

            public Handler(ApplicationDbContext dbContext)
            {
                this.dbContext = dbContext;
            }

            public async Task<Result<CountryResponse>> Handle(Query request, CancellationToken cancellationToken)
            {
                var country = await dbContext.Countries.FindAsync(request.Id);

                if (country == null)
                {
                    return Result.Failure<CountryResponse>(
                        new Error("Country.NotFound", "The country with the provided Id was not found"));
                }

                country.Popularity++;
                await dbContext.SaveChangesAsync(cancellationToken);

                return Result.Success(new CountryResponse
                {
                    Id = country.Id,
                    Name = country.Name,
                    CostOfLivingIndex = country.CostOfLivingIndex,
                    RentIndex = country.RentIndex,
                    CostOfLivingPlusRentIndex = country.CostOfLivingPlusRentIndex,
                    GroceriesIndex = country.GroceriesIndex,
                    RestaurantPriceIndex = country.RestaurantPriceIndex,
                    LocalPurchasingPowerIndex = country.LocalPurchasingPowerIndex,
                    CreatedAt = country.CreatedAt,
                    UpdatedAt = country.UpdatedAt,
                    Popularity = country.Popularity
                });
            }
        }
    }
}

public class UpdateCountryPopularityEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("api/country/increase-popularity", async (Query query, ISender sender) =>
        {
            var result = await sender.Send(query);

            if (result.IsFailure)
            {
                return Results.BadRequest(result.Error);
            }
            return Results.Ok(result.Value);
        }).RequireAuthorization();
    }
}