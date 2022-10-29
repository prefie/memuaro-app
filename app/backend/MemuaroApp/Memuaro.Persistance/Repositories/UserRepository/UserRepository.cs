using Memuaro.Persistance.Client;
using Memuaro.Persistance.Entities;
using Memuaro.Persistance.Repositories.Base;
using MongoDB.Driver;

namespace Memuaro.Persistance.Repositories.UserRepository;

public class UserRepository : BaseRepository<User>, IUserRepository
{
    private const string CollectionName = "users";

    public UserRepository(IDatabaseClient databaseClient) : base(databaseClient, CollectionName)
    {
    }

    public Task<User> GetByEmailAsync(string? email) =>
        MongoCollection.Find(user => user.Email == email).FirstOrDefaultAsync();
}