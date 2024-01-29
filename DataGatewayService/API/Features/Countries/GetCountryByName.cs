using API.Contracts;
using API.Database;
using API.Features.Countries;
using API.Shared;
using Carter;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace API.Features.Countries
{
    public class GetCountryByName
    {
        public class Query : IRequest<Result<CountryResponse>>
        {
            public string Name { get; set; }
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
                return await dbContext.Countries
                    .AsNoTracking()
                    .Where(x=> x.Name == request.Name)
                    .Select(x => new CountryResponse
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
                    }).FirstOrDefaultAsync(cancellationToken);
            }
        }
    }
}

public class GetCountryByNameEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/country", async (string name, ISender sender) =>
        {
            var query = new GetCountryByName.Query { Name = name };
            var result = await sender.Send(query);

            if (result.IsFailure)
            {
                return Results.BadRequest(result.Error);
            }
            return Results.Ok(result.Value);
        }).RequireAuthorization();
    }
}