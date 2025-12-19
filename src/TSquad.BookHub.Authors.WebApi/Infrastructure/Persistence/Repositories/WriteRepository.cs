using TSquad.BookHub.Authors.WebApi.Application.Interface.Persistence;
using TSquad.BookHub.Authors.WebApi.Infrastructure.Persistence.Contexts;

namespace TSquad.BookHub.Authors.WebApi.Infrastructure.Persistence.Repositories;

public class WriteRepository<TEntity> : IWriteRepository<TEntity> where TEntity : class
{
    private readonly ApplicationContextAuthor _context;

    public WriteRepository(ApplicationContextAuthor context)
    {
        _context = context;
    }

    public async Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        var response = await _context.Set<TEntity>().AddAsync(entity, cancellationToken);
        return response.Entity;
    }

    public async Task<bool> UpdateAsync(TEntity entity)
    {
        _context.Set<TEntity>().Update(entity);
        return await Task.FromResult(true);
    }

    public async Task<bool> RemoveAsync(TEntity entity)
    {
        _context.Set<TEntity>().Remove(entity);
        return await Task.FromResult(true);
    }
}