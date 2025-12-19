using System.Linq.Expressions;
using TSquad.BookHub.ShoppingCart.WebApi.Domain.Entities.Base;

namespace TSquad.BookHub.ShoppingCart.WebApi.Application.Interface.Persistence;

public interface IReadRepository<TEntity, in TId> where TEntity : BaseEntity<TId>
{
    IQueryable<TEntity> GetAllAsync();

    Task<IEnumerable<TEntity>> GetAllByPredicateAsync(Expression<Func<TEntity, bool>>? predicate = null,
        CancellationToken cancellationToken = default, bool asNoTracking = true,
        params Expression<Func<TEntity, object>>[] includeProperties);
    
    Task<TEntity?> FindAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default, bool asNoTracking = true, 
        params Expression<Func<TEntity, object>>[] includeProperties);
    
    Task<TEntity?> GetByIdAsync(TId id, CancellationToken cancellationToken = default, bool asNoTracking = true, 
        params Expression<Func<TEntity, object>>[] includeProperties);
}