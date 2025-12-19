using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using TSquad.BookHub.Authors.WebApi.Application.Dto;
using TSquad.BookHub.Authors.WebApi.Application.UseCases.Authors.Commands.CreateAuthorCommand;
using TSquad.BookHub.Authors.WebApi.Application.UseCases.Authors.Queries.GetAllAuthorQuery;
using TSquad.BookHub.Authors.WebApi.Application.UseCases.Authors.Queries.GetAuthorQuery;

namespace TSquad.BookHub.Authors.WebApi.Endpoints;

public static class AuthorsEndpoints
{
    public static IEndpointRouteBuilder MapAuthorsEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/api/authors").WithTags("Authors");

        group.MapGet("", async (IMediator mediator, CancellationToken ct) =>
        {
            var response = await mediator.Send(new GetAllAuthorQuery(), ct);
            return response;
        });
        
        group.MapGet("/{externalId}", async Task<Results<Ok<AuthorDto>, NotFound>> 
            (IMediator mediator, string externalId, CancellationToken ct) =>
        {
            var response = await mediator.Send(new GetAuthorQuery(externalId), ct);
            if(response is null) return TypedResults.NotFound();
            
            return TypedResults.Ok(response);
        });
        
        group.MapPost("", async (CreateAuthorCommand command, IMediator mediator, CancellationToken ct) => 
            {
                var response = await mediator.Send(command, ct);
                return TypedResults.Created($"/{response.ExternalAuthorId}", response);
            })
            .ProducesProblem(StatusCodes.Status400BadRequest);
        
        return group;
    }
}