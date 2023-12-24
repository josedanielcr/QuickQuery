using API.Contracts;
using API.Database;
using API.Dtos;
using API.Entities;
using API.Features.Users;
using API.Shared;
using API.Utilities;
using Carter;
using FluentValidation;
using Mapster;
using MediatR;

namespace API.Features.Users;
public static class CreateUser
{
    public class Command : IRequest<Result<UserDto>>
    {
        public string Username { get; set; } = String.Empty;
        public string Email { get; set; } = String.Empty;
        public string Password { get; set; } = String.Empty;
    }

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.Username).NotEmpty();
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Password).NotEmpty().MinimumLength(12);
        }
    }

    internal sealed class Handler : IRequestHandler<Command, Result<UserDto>>
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IValidator<Command> _validator;

        public Handler(ApplicationDbContext context, IValidator<Command> validator)
        {
            _dbContext = context;
            _validator = validator;
        }

        public async Task<Result<UserDto>> Handle(Command request, CancellationToken cancellationToken)
        {

            var validatorResult = _validator.Validate(request);
            if (!validatorResult.IsValid)
            {
                return Result.Failure<UserDto>(new Error("CreateUser.Validation", validatorResult.ToString()));
            }

            var (success, passwordHash, passwordSalt, message) = TryCreatePasswordHash(request.Password);
            if (!success)
            {
                return Result.Failure<UserDto>(new Error("CreateUser.PasswordHash", message));
            }

            var user = new User
            {
                Id = Guid.NewGuid(),
                Username = request.Username,
                Email = request.Email,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt
            };

            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return user.Adapt<UserDto>();
        }

        private static (bool Success, byte[] PasswordHash, byte[] PasswordSalt, string message) TryCreatePasswordHash(string password)
        {
            try
            {
                SecurityUtils.CreatePasswordHash(password, out var passwordHash, out var passwordSalt);
                return (true, passwordHash, passwordSalt, string.Empty);
            }
            catch (Exception ex)
            {
                return (false, Array.Empty<byte>(), Array.Empty<byte>(), ex.Message);
            }
        }
    }
}
public class CreateUserEndpoint: ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("api/signup", async (CreateUserRequest request, ISender sender) =>
        {

            var command = request.Adapt<CreateUser.Command>();

            var result = await sender.Send(command);

            if (result.IsFailure)
            {
                return Results.BadRequest(result.Error);
            }

            return Results.Ok(result.Value);
        });
    }
}