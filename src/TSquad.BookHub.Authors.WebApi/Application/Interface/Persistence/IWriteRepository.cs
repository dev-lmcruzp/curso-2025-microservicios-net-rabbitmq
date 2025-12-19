namespace TSquad.BookHub.Authors.WebApi.Application.Interface.Persistence;

public interface IWriteRepository<TEntity> where TEntity : class
{
    Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(TEntity entity);
    Task<bool> RemoveAsync(TEntity entity);
}