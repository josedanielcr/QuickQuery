using API.Contracts;
using API.Database;
using API.Dtos;
using API.Entities;
using API.Shared;
using API.Utilities;
using Carter;
using FluentValidation;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace API.Features.Users;

public static class LoginUser
{
    public class Command : IRequest<Result<LoginResultDto>>
    {
        public string Email { get; set; } = String.Empty;
        public string Password { get; set; } = String.Empty;
    }

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.Email).NotEmpty();
            RuleFor(x => x.Password).NotEmpty().MinimumLength(12);
        }
    }

    internal sealed class Handler : IRequestHandler<Command, Result<LoginResultDto>>
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IValidator<Command> _validator;

        public Handler(ApplicationDbContext context, IValidator<Command> validator)
        {
            _dbContext = context;
            _validator = validator;
        }

        public async Task<Result<LoginResultDto>> Handle(Command request, CancellationToken cancellationToken)
        {

            var validatorResult = _validator.Validate(request);
            if (!validatorResult.IsValid)
            {
                return Result.Failure<LoginResultDto>(new Error("LoginUser.Validation", validatorResult.ToString()));
            }

            var user = await ValidateIfUserExists(request.Email, cancellationToken, request);
            if (user == null)
            {
                return Result.Failure<LoginResultDto>(new Error("LoginUser.NotFound", "User not found"));
            }

            var result = ValidatePassword(request.Password, user);
            if (!result)
            {
                return Result.Failure<LoginResultDto>(new Error("LoginUser.InvalidPassword", "Invalid password"));
            }

            var token = GetToken(user);
            return Result.Success(new LoginResultDto
            {
                Token = token,
                User = user.Adapt<UserDto>()
            });

        }

        private async Task<User> ValidateIfUserExists(string email, CancellationToken cancellationToken, Command request)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Email == request.Email, cancellationToken);
            return user!;
        }

        private bool ValidatePassword(string password, User user)
        {
            try
            {
                return SecurityUtils.VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private string GetToken(User user)
        {
            try
            {
                return SecurityUtils.GenerateJwtToken(user);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}

public class LoginUserEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("api/login", async (LoginResultRequest request, ISender sender) =>
        {
            var command = request.Adapt<LoginUser.Command>();

            var result = await sender.Send(command);

            if (result.IsFailure)
            {
                return Results.BadRequest(result.Error);
            }

            return Results.Ok(result.Value);
        });
    }
}