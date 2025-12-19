using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TSquad.BookHub.ShoppingCart.WebApi.Application.Interface.Persistence;
using TSquad.BookHub.ShoppingCart.WebApi.Domain.Entities.Base;
using TSquad.BookHub.ShoppingCart.WebApi.Infrastructure.Persistence.Contexts;

namespace TSquad.BookHub.ShoppingCart.WebApi.Infrastructure.Persistence.Repositories;

public class ReadRepository<TEntity, TId> : IReadRepository<TEntity, TId> where TEntity : BaseEntity<TId>
{
    private readonly ApplicationContextShoppingCart _context;

    public ReadRepository(ApplicationContextShoppingCart context)
    {
        _context = context;
    }

    public IQueryable<TEntity> GetAllAsync()
    {
        return _context.Set<TEntity>();
    }

    public async Task<IEnumerable<TEntity>> GetAllByPredicateAsync(Expression<Func<TEntity, bool>>? predicate = null, CancellationToken cancellationToken = default, bool asNoTracking = true,
        params Expression<Func<TEntity, object>>[] includeProperties)
    {
        var query = _context.Set<TEntity>().AsQueryable();
        if(predicate is not null)
            query = query.Where(predicate);
        
        query = ApplyAsNoTrackingInclude(query, asNoTracking, includeProperties);

        return await query.ToListAsync(cancellationToken);
    }

    public async Task<TEntity?> FindAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default, bool asNoTracking = true,
        params Expression<Func<TEntity, object>>[] includeProperties)
    {
        var query = _context.Set<TEntity>().AsQueryable();
        query = ApplyAsNoTrackingInclude(query, asNoTracking, includeProperties);
        return await query.FirstOrDefaultAsync(predicate, cancellationToken);
    }

    public async Task<TEntity?> GetByIdAsync(TId id, CancellationToken cancellationToken = default, bool asNoTracking = true,
        params Expression<Func<TEntity, object>>[] includeProperties)
    {
        var query = _context.Set<TEntity>().AsQueryable();
        query = ApplyAsNoTrackingInclude(query, asNoTracking, includeProperties);
        return await query.FirstOrDefaultAsync(x => x.Id.Equals(id), cancellationToken) ;
    }

    private IQueryable<TEntity> ApplyAsNoTrackingInclude(IQueryable<TEntity> query, bool asNoTracking,
        Expression<Func<TEntity, object>>[] includeProperties)
    {
        if (asNoTracking)
            query = query.AsNoTracking();

        query = includeProperties.Aggregate(query, 
            (current, includeProperty) => current.Include(includeProperty));

        return query;
    }
}