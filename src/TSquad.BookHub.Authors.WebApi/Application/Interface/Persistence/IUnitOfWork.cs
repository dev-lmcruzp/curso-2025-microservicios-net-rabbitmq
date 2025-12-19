using TSquad.BookHub.Authors.WebApi.Domain.Entities;

namespace TSquad.BookHub.Authors.WebApi.Application.Interface.Persistence;

public interface IUnitOfWork : IDisposable
{
    // IReadRepository<TEntity, TId> ReadRepository<TEntity, TId>() where TEntity : BaseEntity<TId>;
    // IWriteRepository<TEntity, TId> WriteRepository<TEntity, TId>() where TEntity : BaseEntity<TId>;

    IReadRepository<Author, long> AuthorReadRepository { get; }
    IWriteRepository<Author> AuthorWriteRepository { get; }

    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}