using MediatR;
using TSquad.BookHub.ShoppingCart.WebApi.Application.Dto;
using TSquad.BookHub.ShoppingCart.WebApi.Application.Interface.Infrastructure.Services;
using TSquad.BookHub.ShoppingCart.WebApi.Application.Interface.Persistence;

namespace TSquad.BookHub.ShoppingCart.WebApi.Application.UseCases.ShoppingCartSessions.Queries.ShoppingCartQuery;

public sealed record ShoppingCartQuery(long ShoppingCartId) : IRequest<ShoppingCartDto?>;

public class ShoppingCartHandler : IRequestHandler<ShoppingCartQuery, ShoppingCartDto?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IBookService _bookService;

    public ShoppingCartHandler(IUnitOfWork unitOfWork, IBookService bookService)
    {
        _unitOfWork = unitOfWork;
        _bookService = bookService;
    }

    public async Task<ShoppingCartDto?> Handle(ShoppingCartQuery request, CancellationToken cancellationToken)
    {
        var shoppingCart = await _unitOfWork.ShoppingCartSessionReadRepository
            .GetByIdAsync(request.ShoppingCartId, cancellationToken, false, c => c.ShoppingCartItems);

        if (shoppingCart is null)
            return null;
        
        var result = new ShoppingCartDto()
        {
            ShoppingCartId = shoppingCart.Id,
            CreatedAt = shoppingCart.CreatedAt,
            Items = []
        };
        
        if(shoppingCart.ShoppingCartItems.Count == 0)
            return result;
        
        /*foreach (var shoppingCartItem in shoppingCartItems)
        {
            var responseBook = await _bookService.GetBook(shoppingCartItem.ProductId);
            if(!responseBook.result)
                continue;

            result.Items.Add(new ShoppingCartItemDto()
            {
                BookId =  responseBook.book!.ExternalBookId,
                Author = responseBook.book!.AuthorId,
                DatePublic = responseBook.book!.DatePublic,
                Title = responseBook.book!.Title
            });
        }*/
        
        var semaphore = new SemaphoreSlim(5);
        
        // Para evitar error 429
        var tasks = shoppingCart.ShoppingCartItems.Select(async cartItem =>
        {
            await semaphore.WaitAsync(cancellationToken);
            try
            {
                return await _bookService.GetBook(cartItem.ProductId, cancellationToken);
            }
            finally
            {
                semaphore.Release();
            }
        }).ToList();
            
        // tasks.AddRange(shoppingCart.ShoppingCartItems
        //.Select(cartItem => _bookService.GetBook(cartItem.ProductId, cancellationToken)));
        
        var products = await Task.WhenAll(tasks);
        result.Items = products.Where(x => x.result & x.book is not null)
            .Select(x => new ShoppingCartItemDto()
            {
                BookId = x.book!.ExternalBookId,
                Author = x.book!.AuthorId,
                DatePublic = x.book!.DatePublic,
                Title = x.book!.Title
            }).ToList();
        
        return result;
    }
}