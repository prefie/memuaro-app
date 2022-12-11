using MongoDB.Driver;

namespace Rememory.Persistance.Client;

public interface IDatabaseClient
{
    IClientSessionHandle GetSession();
    IMongoCollection<T> GetCollection<T>(string collectionName);
}