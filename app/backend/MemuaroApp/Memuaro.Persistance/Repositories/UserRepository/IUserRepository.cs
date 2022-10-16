using Memuaro.Persistance.Entities;
using Memuaro.Persistance.Repositories.Base;

namespace Memuaro.Persistance.Repositories.UserRepository;

public interface IUserRepository : IRepository<User>
{
    public Task<User> GetByEmailAsync(string email);
}