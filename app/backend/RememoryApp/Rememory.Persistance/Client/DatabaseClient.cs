using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Rememory.Persistance.Client;

public class DatabaseClient : IDatabaseClient
{
    private readonly IMongoDatabase _database;
    private readonly IMongoClient _mongoClient;

    public DatabaseClient(IOptions<DatabaseConfig> databaseConfig)
    {
        _mongoClient = new MongoClient(databaseConfig.Value.ConnectionString);
        _database = _mongoClient.GetDatabase(databaseConfig.Value.DatabaseName);
    }

    public IClientSessionHandle GetSession() => _mongoClient.StartSession();
    
    public IMongoCollection<T> GetCollection<T>(string collectionName) => _database.GetCollection<T>(collectionName);
}