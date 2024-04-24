using QuickqueryDataGatewayAPI.Contracts;
using QuickqueryDataGatewayAPI.Database;
using QuickqueryDataGatewayAPI.Entities;
using QuickqueryDataGatewayAPI.Features.CountrySearchLogs;
using QuickqueryDataGatewayAPI.Shared;
using Carter;
using Mapster;
using MediatR;
using static QuickqueryDataGatewayAPI.Features.CountrySearchLogs.CreateCountrySearchLog;

namespace QuickqueryDataGatewayAPI.Features.CountrySearchLogs
{
    public class CreateCountrySearchLog
    {
        public class Command : IRequest<Result>
        {
            public Guid UserId { get; set; }
            public Guid CountryId { get; set; }
        }

        internal sealed class Handler : IRequestHandler<Command, Result>
        {
            private readonly ApplicationDbContext dbContext;

            public Handler(ApplicationDbContext dbContext)
            {
                this.dbContext = dbContext;
            }

            public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
            {
                CountrySearchLog countrySearchLog = CreateCountrySearchLogInstance(request);
                return await SaveCountrySearchLogIntoDB(countrySearchLog, cancellationToken);
            }

            private async Task<Result> SaveCountrySearchLogIntoDB(CountrySearchLog countrySearchLog, CancellationToken cancellationToken)
            {
                try
                {
                    dbContext.CountriesSearchLog.Add(countrySearchLog);
                    await dbContext.SaveChangesAsync(cancellationToken);
                    return Result.Success();
                }
                catch (Exception e)
                {
                    return Result.Failure(new Error("Database.SaveError", e.Message));
                }
            }

            private static CountrySearchLog CreateCountrySearchLogInstance(Command request)
            {
                return new CountrySearchLog
                {
                    Id = Guid.NewGuid(),
                    UserId = request.UserId,
                    CountryId = request.CountryId,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = null
                };
            }
        }
    }
}

public class CreateCountrySearchLogEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("api/country/search-log", async (CreateCountrySearchLogRequest request, ISender sender) =>
        {
            var command = request.Adapt<CreateCountrySearchLog.Command>();
            var result = await sender.Send(command);

            if (result.IsFailure)
            {
                return Results.BadRequest(result.Error);
            }
            return Results.Ok(result);
        }).RequireAuthorization();
    }
}