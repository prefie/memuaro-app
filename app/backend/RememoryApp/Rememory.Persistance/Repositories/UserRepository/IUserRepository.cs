using Rememory.Persistance.Entities;
using Rememory.Persistance.Repositories.Base;

namespace Rememory.Persistance.Repositories.UserRepository;

public interface IUserRepository : IRepository<User>
{
    public Task<User> GetByEmailAsync(string? email);
}