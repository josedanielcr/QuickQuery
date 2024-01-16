using API.Contracts;
using API.Database;
using API.Features.Users;
using API.Shared;
using Carter;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace API.Features.Users
{
    public static class GetUserByEmail
    {

        public class Query : IRequest<Result<UserResponse>>
        {
            public string Email { get; set; } = String.Empty;
        }

        internal sealed class Handler : IRequestHandler<Query, Result<UserResponse>>
        {
            private readonly ApplicationDbContext dbContext;

            public Handler(ApplicationDbContext dbContext)
            {
                this.dbContext = dbContext;
            }

            public async Task<Result<UserResponse>> Handle(Query request, CancellationToken cancellationToken)
            {
                UserResponse? result = await QueryUserByRequestData(request, cancellationToken);

                return ManageUserQueryResult(result);
            }

            private async Task<UserResponse?> QueryUserByRequestData(Query request, CancellationToken cancellationToken)
            {
                return await dbContext.Users
                    .AsNoTracking()
                    .Where(x => x.Email == request.Email)
                    .Select(user => new UserResponse
                    {
                        Id = user.Id,
                        Email = user.Email,
                        Username = user.Username
                    })
                    .FirstOrDefaultAsync(cancellationToken);
            }

            private static Result<UserResponse> ManageUserQueryResult(UserResponse? result)
            {
                if (result == null)
                {
                    return Result.Failure<UserResponse>(new Error("GetUser.NotFound", "User not found"));
                }

                return result;
            }
        }
    }
}

public class GetUserEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/users/{email}", async (string email, ISender sender) =>
        {
            var query = new GetUserByEmail.Query { Email = email };
            var result = await sender.Send(query);

            if(result.IsFailure)
            {
                return Results.NotFound(result.Error);
            }

            return Results.Ok(result.Value);

        });
    }
}