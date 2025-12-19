using TSquad.BookHub.ShoppingCart.WebApi.Domain.Entities;

namespace TSquad.BookHub.ShoppingCart.WebApi.Application.Interface.Persistence;

public interface IUnitOfWork : IDisposable
{
    IWriteRepository<ShoppingCartSession> ShoppingCartSessionWriteRepository { get; }
    IReadRepository<ShoppingCartSession, long> ShoppingCartSessionReadRepository { get; }
    
    IWriteRepository<ShoppingCartItem> ShoppingCartItemWriteRepository { get; }
    IReadRepository<ShoppingCartItem, long> ShoppingCartItemReadRepository { get; }
    
    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}