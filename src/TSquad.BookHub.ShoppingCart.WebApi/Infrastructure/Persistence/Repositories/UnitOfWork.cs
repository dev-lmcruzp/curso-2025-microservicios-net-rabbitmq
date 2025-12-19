using Microsoft.EntityFrameworkCore.Storage;
using TSquad.BookHub.ShoppingCart.WebApi.Domain.Entities;
using TSquad.BookHub.ShoppingCart.WebApi.Application.Interface.Persistence;
using TSquad.BookHub.ShoppingCart.WebApi.Infrastructure.Persistence.Contexts;

namespace TSquad.BookHub.ShoppingCart.WebApi.Infrastructure.Persistence.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationContextShoppingCart _context;
    private IDbContextTransaction? _currentTransactionAsync;

    public UnitOfWork(ApplicationContextShoppingCart context, 
        IWriteRepository<ShoppingCartSession> bookWriteRepository, IReadRepository<ShoppingCartSession, long> bookReadRepository, IWriteRepository<ShoppingCartItem> shoppingCartItemWriteRepository, IReadRepository<ShoppingCartItem, long> shoppingCartItemReadRepository)
    {
        _context = context;
        ShoppingCartSessionWriteRepository = bookWriteRepository;
        ShoppingCartSessionReadRepository = bookReadRepository;
        ShoppingCartItemWriteRepository = shoppingCartItemWriteRepository;
        ShoppingCartItemReadRepository = shoppingCartItemReadRepository;
    }


    public void Dispose()
    {
        _context.Dispose();
    }

    public IWriteRepository<ShoppingCartSession> ShoppingCartSessionWriteRepository { get; }
    public IReadRepository<ShoppingCartSession, long> ShoppingCartSessionReadRepository { get; }
    public IWriteRepository<ShoppingCartItem> ShoppingCartItemWriteRepository { get; }
    public IReadRepository<ShoppingCartItem, long> ShoppingCartItemReadRepository { get; }

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