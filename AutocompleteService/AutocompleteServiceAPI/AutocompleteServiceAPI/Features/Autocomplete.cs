using AutocompleteServiceAPI.DataStructures;
using AutocompleteServiceAPI.Features;
using AutocompleteServiceAPI.Shared;
using Carter;
using MediatR;
using System.Xml.Linq;

namespace AutocompleteServiceAPI.Features
{
    public class Autocomplete
    {
        //Query
        public class Query : IRequest<Result<List<string>>>
        {
            public string Prefix { get; set; }
        }

        //Handler
        internal sealed class Handler : IRequestHandler<Query, Result<List<string>>>
        {
            public async Task<Result<List<string>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var result = Trie.GetWords(request.Prefix);
                return Result.Success(result);
            }
        }
    }
}


public class AutocompleteEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/autocomplete", async (string prefix, ISender sender) =>
        {
            var query = new Autocomplete.Query { Prefix = prefix };
            var result = await sender.Send(query);

            if (result.IsFailure)
            {
                return Results.BadRequest(result.Error);
            }
            return Results.Ok(result.Value);
        }).RequireAuthorization();
    }
}