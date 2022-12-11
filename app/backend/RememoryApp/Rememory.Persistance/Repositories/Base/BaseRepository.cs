using MongoDB.Driver;
using Rememory.Persistance.Client;
using Rememory.Persistance.Entities;

namespace Rememory.Persistance.Repositories.Base;

public abstract class BaseRepository<T>
    where T : IDatabaseEntity
{
    protected readonly IMongoCollection<T> MongoCollection;

    protected BaseRepository(
        IDatabaseClient databaseClient,
        string collectionName)
    {
        MongoCollection = databaseClient.GetCollection<T>(collectionName);
    }

    public Task<List<T>> GetAsync() => MongoCollection.Find(_ => true).ToListAsync();
    
    public Task<T> GetAsync(Guid id) => MongoCollection.Find(entity => entity.Id == id).FirstOrDefaultAsync();

    public Task CreateAsync(T entity) => MongoCollection.InsertOneAsync(entity);

    public Task UpdateAsync(Guid id, T updatedEntity) =>
        MongoCollection.ReplaceOneAsync(entity => entity.Id == id, updatedEntity);

    public Task RemoveAsync(Guid id) => MongoCollection.DeleteOneAsync(entity => entity.Id == id);
}