using System.Diagnostics;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using TSquad.BookHub.ShoppingCart.WebApi.Application.Dto;
using TSquad.BookHub.ShoppingCart.WebApi.Application.UseCases.ShoppingCartSessions.Commands.ShoppingCartSessionCommand;
using TSquad.BookHub.ShoppingCart.WebApi.Application.UseCases.ShoppingCartSessions.Queries.ShoppingCartQuery;

namespace TSquad.BookHub.ShoppingCart.WebApi.Endpoints;

public static class ShoppingCartsEndpoints
{
    public static IEndpointRouteBuilder MapShoppingCartsEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/api/shopping-carts").WithTags("ShoppingCarts");

        /*group.MapGet("", async (IMediator mediator, CancellationToken ct) =>
            {
                var response = await mediator.Send(new GetAllBookQuery(), ct);
                return response;
            })
            .ProducesProblem(StatusCodes.Status400BadRequest);*/

        group.MapGet("/{id:long}",
                async Task<Results<Ok<ShoppingCartDto>, NotFound>> (IMediator mediator, long id, CancellationToken ct) =>
                {
                    var stopwatch = new Stopwatch();
                    stopwatch.Start();
                    
                    var response = await mediator.Send(new ShoppingCartQuery(id), ct);
                    
                    stopwatch.Stop();
                    Console.WriteLine($"\n\nEl método tomó: {stopwatch.ElapsedMilliseconds} milisegundos.\n\n");
                    
                    if (response is null)
                        return TypedResults.NotFound();

                    return TypedResults.Ok(response);
                })
            .ProducesProblem(StatusCodes.Status400BadRequest);

        group.MapPost("", async (IMediator mediator, ShoppingCartSessionCommand command, CancellationToken ct) =>
            {
                var response = await mediator.Send(command, ct);
                // return TypedResults.Created($"/{response.Id}", response);
                return TypedResults.Created("", response);
            })
            .ProducesProblem(StatusCodes.Status400BadRequest);

        return group;
    }
}