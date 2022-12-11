using Rememory.Persistance.Entities;

namespace Rememory.Persistance.Repositories.Base;

public interface IRepository<T>
    where T : IDatabaseEntity
{
    public Task<List<T>> GetAsync();
    public Task<T> GetAsync(Guid id);
    public Task CreateAsync(T entity);
    public Task UpdateAsync(Guid id, T updatedEntity);
    public Task RemoveAsync(Guid id);
}