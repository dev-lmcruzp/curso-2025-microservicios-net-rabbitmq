using Microsoft.EntityFrameworkCore.Storage;
using TSquad.BookHub.Books.WebApi.Application.Interface.Persistence;
using TSquad.BookHub.Books.WebApi.Domain.Entities;
using TSquad.BookHub.Books.WebApi.Infrastructure.Persistence.Contexts;

namespace TSquad.BookHub.Books.WebApi.Infrastructure.Persistence.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationContextBook _context;
    private IDbContextTransaction? _currentTransactionAsync;

    public UnitOfWork(ApplicationContextBook context, 
        IWriteRepository<Book> bookWriteRepository, IReadRepository<Book, long> bookReadRepository)
    {
        _context = context;
        BookWriteRepository = bookWriteRepository;
        BookReadRepository = bookReadRepository;
    }


    public void Dispose()
    {
        _context.Dispose();
    }

    public IWriteRepository<Book> BookWriteRepository { get; }
    public IReadRepository<Book, long> BookReadRepository { get; }

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