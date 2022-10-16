using MongoDB.Driver;

namespace Memuaro.Persistance.Client;

public interface IDatabaseClient
{
    IClientSessionHandle GetSession();
    IMongoCollection<T> GetCollection<T>(string collectionName);
}