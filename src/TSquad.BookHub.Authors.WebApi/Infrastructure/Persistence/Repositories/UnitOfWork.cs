using Microsoft.EntityFrameworkCore.Storage;
using TSquad.BookHub.Authors.WebApi.Application.Interface.Persistence;
using TSquad.BookHub.Authors.WebApi.Domain.Entities;
using TSquad.BookHub.Authors.WebApi.Infrastructure.Persistence.Contexts;

namespace TSquad.BookHub.Authors.WebApi.Infrastructure.Persistence.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationContextAuthor _context;
    private IDbContextTransaction? _currentTransactionAsync;

    public UnitOfWork(ApplicationContextAuthor context, 
        IWriteRepository<Author> authorWriteRepository, IReadRepository<Author, long> authorReadRepository)
    {
        _context = context;
        AuthorWriteRepository = authorWriteRepository;
        AuthorReadRepository = authorReadRepository;
    }


    public void Dispose()
    {
        _context.Dispose();
    }

    public IWriteRepository<Author> AuthorWriteRepository { get; }
    public IReadRepository<Author, long> AuthorReadRepository { get; }

    public async Task BeginTransactionAsync()
    {
        if(_currentTransactionAsync is not null)
            return;
        
        _currentTransactionAsync = await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync()
    {
        if(_currentTransactionAsync is null)
            return;

        await _context.SaveChangesAsync();
        await _currentTransactionAsync.CommitAsync();
        ResetTransactionAsync();
    }

    public async Task RollbackTransactionAsync()
    {
        if(_currentTransactionAsync is null)
            return;
        
        await _currentTransactionAsync.RollbackAsync();
        ResetTransactionAsync();
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    private void ResetTransactionAsync()
    {
        _currentTransactionAsync?.Dispose();
        _currentTransactionAsync = null;
    }
}