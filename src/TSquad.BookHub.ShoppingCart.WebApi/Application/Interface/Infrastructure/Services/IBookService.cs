using TSquad.BookHub.ShoppingCart.WebApi.Application.Dto.Remote;

namespace TSquad.BookHub.ShoppingCart.WebApi.Application.Interface.Infrastructure.Services;

public interface IBookService
{
    Task<(bool result, RemoteBookDto? book, string? error)> GetBook(string bookId, CancellationToken cancellationToken);
}