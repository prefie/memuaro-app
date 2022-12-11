using MongoDB.Driver;
using Rememory.Persistance.Client;
using Rememory.Persistance.Entities;
using Rememory.Persistance.Repositories.Base;

namespace Rememory.Persistance.Repositories.UserRepository;

public class UserRepository : BaseRepository<User>, IUserRepository
{
    private const string CollectionName = "users";

    public UserRepository(IDatabaseClient databaseClient) : base(databaseClient, CollectionName)
    {
    }

    public Task<User> GetByEmailAsync(string? email) =>
        MongoCollection.Find(user => user.Email == email).FirstOrDefaultAsync();
}