using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using TSquad.BookHub.Books.WebApi.Application.Dto;
using TSquad.BookHub.Books.WebApi.Application.UseCases.Books.Commands.CreateBookCommand;
using TSquad.BookHub.Books.WebApi.Application.UseCases.Books.Queries.GetAllBookQuery;
using TSquad.BookHub.Books.WebApi.Application.UseCases.Books.Queries.GetBookQuery;

namespace TSquad.BookHub.Books.WebApi.Endpoints;

public static class BooksEndpoints
{
    public static IEndpointRouteBuilder MapBooksEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/api/books").WithTags("Books");

        group.MapGet("", async (IMediator mediator, CancellationToken ct) =>
            {
                var response = await mediator.Send(new GetAllBookQuery(), ct);
                return response;
            })
            .ProducesProblem(StatusCodes.Status400BadRequest);

        group.MapGet("/{id}",
                async Task<Results<Ok<BookDto>, NotFound>> (IMediator mediator, string id, CancellationToken ct) =>
                {
                    var response = await mediator.Send(new GetBookQuery(id), ct);
                    if (response is null)
                        return TypedResults.NotFound();

                    return TypedResults.Ok(response);
                })
            .ProducesProblem(StatusCodes.Status400BadRequest);

        group.MapPost("", async (IMediator mediator, CreateBookCommand command, CancellationToken ct) =>
            {
                var response = await mediator.Send(command, ct);
                return TypedResults.Created($"/{response.ExternalBookId}", response);
            })
            .ProducesProblem(StatusCodes.Status400BadRequest);

        return group;
    }
}