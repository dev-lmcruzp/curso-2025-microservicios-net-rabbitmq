using TSquad.BookHub.Books.WebApi.Domain.Entities;

namespace TSquad.BookHub.Books.WebApi.Application.Interface.Persistence;

public interface IUnitOfWork : IDisposable
{
    IWriteRepository<Book> BookWriteRepository { get; }
    IReadRepository<Book, long> BookReadRepository { get; }
    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}